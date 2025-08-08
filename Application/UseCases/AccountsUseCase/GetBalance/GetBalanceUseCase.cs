using Domain.Exceptions;
using Domain.Interfaces.Repositories;

namespace Application.UseCases.AccountsUseCase.GetBalance
{
    public class GetBalanceUseCase : IGetBalanceUseCase
    {
        private readonly IContaRepository _contaRepository;
        public GetBalanceUseCase(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }
        public async Task<Response> HandleAsync(string accountId)
        {
            var conta = await _contaRepository.SingleAsync(x => x.Id == accountId);

            if (conta == null)
                throw new ApiException("Conta não encontrado.");

            var balance = conta.Transactions.Sum(t => t.Value);

            return new Response(
                Math.Round(balance, 2)
            );
        }

    }

    public record Response(decimal Balance);
}
