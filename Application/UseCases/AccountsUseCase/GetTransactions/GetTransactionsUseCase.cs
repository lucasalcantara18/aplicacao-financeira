using Domain.Dtos;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;

namespace Application.UseCases.AccountsUseCase.GetTransactions
{
    public class GetTransactionsUseCase : IGetTransactionsUseCase
    {
        private readonly IContaRepository _contaRepository;
        public GetTransactionsUseCase(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }
        public async Task<Response> HandleAsync(int itemsPerPage, int currentPage, string type, string accountId)
        {
            var conta = await _contaRepository.SingleAsync(x => x.Id == accountId);

            if (conta == null)
                throw new ApiException("Conta não encontrado.");

            var response = conta.Transactions.OrderByDescending(x => x.CreatedAt).Select(x =>
            {
                return new DataResponse(
                    x.Id,
                    Math.Round(x.Value, 2),
                    x.Description,
                    x.CreatedAt,
                    x.UpdatedAt
                );
            });

            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "credit")
                    response = response.Where(x => x.Value > 0);
                else if (type.ToLower() == "debit")
                    response = response.Where(x => x.Value < 0);
            }

            var paginacao = new Pagination(response.Count(), currentPage, itemsPerPage);
            response = response
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            return new Response(
                response,
                paginacao
            );
        }

    }

    public record DataResponse(string Id, decimal Value, string Description, DateTime CreatedAt, DateTime UpdatedAt);

    public record Response(IEnumerable<DataResponse> Transactions, Pagination Pagination);
}
