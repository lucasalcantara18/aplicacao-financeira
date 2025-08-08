using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Application.Services;
using Domain.Exceptions;
using System.Transactions;
using System.Data.Common;

namespace Application.UseCases.AccountsUseCase.RevertTransaction
{
    public class RevertTransactionUseCase : IRevertTransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Dictionary<bool, Func<ClientTransaction, ClientTransaction, Task<Response>>> _transactionHandler = new();

        public RevertTransactionUseCase(IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
        {
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _transactionHandler = new Dictionary<bool, Func<ClientTransaction, ClientTransaction, Task<Response>>>
            {
                { true, HandlerBasicTransaction },
                { false, HandlerInternalTransaction }
            };
        }

        public async Task<object> HandleAsync(string accountId, string transactionId)
        {
            var originalTransaction = await _transactionRepository.SingleAsync(x => x.Id == transactionId);
            var targetlTransaction = await _transactionRepository.SingleAsync(x => x.Id == originalTransaction.Internal);

            if (originalTransaction == null)
                throw new ApiException("Transação não encontrada");

            if (originalTransaction.IsReverted)
                throw new ApiException("Transação já revertida");

            var simpleTransaction = string.IsNullOrEmpty(originalTransaction.Internal);

            if (_transactionHandler.TryGetValue(simpleTransaction, out var executeAsync))
            {
                var response = await executeAsync(originalTransaction, targetlTransaction);
                SetRevertTransactionIndication(originalTransaction, targetlTransaction);
                await _unitOfWork.SaveAsync();
                return response;
            }

            throw new ApiException("Tipo de transação não suportado");
        }

        private async Task<Response> HandlerBasicTransaction(ClientTransaction originalTransaction, ClientTransaction targetTransaction)
        {
            //Transações basicas podem ser do tipo Credito ou Debito.
            //Sendo a transação a ser revertida for debito, (valor < 0), deve ser criar um credito não é necessario validar balanço
            //Sendo a transação a ser revertida for credito, (valor > 0), deve ser criar um debito, é necessario validar o balanço da conta.
            ClientTransaction operation = null;
            if (originalTransaction.Value < 0)
            {
                operation = new ClientTransaction(Math.Abs(originalTransaction.Value), "Estorno de cobrança indevida.", originalTransaction.ContaId);
                originalTransaction.Conta.Transactions.Add(operation);

            }
            else
            {
                var balance = originalTransaction.Conta.Transactions.Sum(x => x.Value);
                if (balance < originalTransaction.Value)
                    throw new ApiException("Saldo insuficiente para estorno da transação.");

                operation = new ClientTransaction(decimal.Negate(originalTransaction.Value), "Estorno de cobrança indevida.", originalTransaction.ContaId);
                originalTransaction.Conta.Transactions.Add(operation);
            }

            return CreateResponse(operation, originalTransaction.Value);
        }

        private async Task<Response> HandlerInternalTransaction(ClientTransaction originalTransaction, ClientTransaction targetTransaction)
        {
            //É definido na rota /accounts/:accountId/transactions/internal que todas as transações internas são do tipo
            //debito, portanto não é necessário verificar o valor da transação original, apenas criar a operação de credito na conta de origem.
            //Fica apenas necessario verificar o saldo da conta de destino da operação original.

            var targetBalance = targetTransaction.Conta.Transactions.Sum(x => x.Value);

            if(targetBalance - (Math.Abs(originalTransaction.Value)) < 0)
                throw new ApiException("Saldo insuficiente na conta de destino para estorno da transação.");

            var creditOriginalAccount = new ClientTransaction(Math.Abs(originalTransaction.Value), "Estorno de cobrança indevida.", originalTransaction.ContaId);
            var debitDestinationAccount = new ClientTransaction(originalTransaction.Value, "Estorno de transferência indevida.", targetTransaction.ContaId);

            originalTransaction.Conta.Transactions.Add(creditOriginalAccount);
            targetTransaction.Conta.Transactions.Add(debitDestinationAccount);

            var respose = CreateResponse(creditOriginalAccount, originalTransaction.Value);
            return respose;
        }

        private static Response CreateResponse(ClientTransaction operation, decimal originalValue)
        {
            return new Response(
                operation.Id,
                Math.Round(originalValue, 2),
                operation.Description,
                operation.CreatedAt,
                operation.UpdatedAt
            );
        }

        private static void SetRevertTransactionIndication(ClientTransaction originalTransaction, ClientTransaction targetlTransaction)
        {
            originalTransaction.IsReverted = true;
            if(targetlTransaction is ClientTransaction)
                targetlTransaction.IsReverted = true;
        }
    }

    public record Response(string Id, decimal Value, string Description, DateTime CreatedAt, DateTime UpdatedAt);
}
