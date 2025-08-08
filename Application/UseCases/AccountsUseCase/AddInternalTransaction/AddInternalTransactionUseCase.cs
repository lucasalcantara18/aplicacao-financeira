using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Domain.Entidades;
using Application.Services;
using Newtonsoft.Json;
using Application.Services.External.ComplianceService;
using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Application.UseCases.AccountsUseCase.AddInternalTransaction
{
    public class AddInternalTransactionUseCase : IAddInternalTransactionUseCase
    {
        private readonly IContaRepository _contaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddInternalTransactionUseCase(IContaRepository contaRepository, IUnitOfWork unitOfWork)
        {
            _contaRepository = contaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> HandleAsync(decimal value, string description, string accountId, string receiverAccountId)
        {
            if (value < 0)
                throw new ApiException("Apenas operações de debito em transação interna.");

            var originalAccount = await _contaRepository.SingleAsync(x => x.Id == accountId);
            var targetAccount = await _contaRepository.SingleAsync(x => x.Id == receiverAccountId);

            if (originalAccount == null || targetAccount == null)
            {
                var accountNotFound = originalAccount == null ? accountId : receiverAccountId;
                throw new ApiException($"Conta {accountNotFound} não encontrado.");
            }

            var originalBalance = originalAccount.Transactions.Sum(t => t.Value);

            if ((originalBalance - value) < 0)
                throw new ApiException("Saldo insuficiente para realizar a transação.");

            
            var originalTransaction = new ClientTransaction((decimal.Negate(value)), description, accountId);
            var targetTransaction = new ClientTransaction(value, description, receiverAccountId);

            originalTransaction.AddInternal(targetTransaction.Id);
            targetTransaction.AddInternal(originalTransaction.Id);

            originalAccount.Transactions.Add(originalTransaction);
            targetAccount.Transactions.Add(targetTransaction);

            await _unitOfWork.SaveAsync();

            return new Response(
                originalTransaction.Id,
                value,
                originalTransaction.Description,
                originalTransaction.CreatedAt,
                originalTransaction.UpdatedAt
            );
        }

    }

    public record Response(string Id, decimal Value, string Description, DateTime CreatedAt, DateTime UpdatedAt);
}
