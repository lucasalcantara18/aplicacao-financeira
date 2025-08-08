using Domain.Interfaces.Repositories;
using Domain.Dtos;
using Domain.Exceptions;

namespace Application.UseCases.CardsUseCase.GetCards
{
    public class GetUserCardsUseCase : IGetUserCardsUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        public GetUserCardsUseCase(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }
        public async Task<Response> HandleAsync(int itemsPerPage, int currentPage, string clientId)
        {
            var pessoa = await _pessoaRepository.SingleAsync(x => x.Id == clientId);

            if (pessoa == null)
                throw new ApiException("Cliente não encontrado.");

            var response = pessoa.Contas.SelectMany(x => x.Cartoes).OrderByDescending(x => x.CreatedAt).Select(x =>
            {
                return new DataResponse(
                    x.Id,
                    x.Type,
                    x.Number.Split(" ").LastOrDefault(),
                    x.Cvv,
                    x.CreatedAt,
                    x.UpdatedAt
                );
            });

            var paginacao = new Pagination(response.Count(), currentPage, itemsPerPage);
            response = response
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);


            return new Response(response, paginacao);
        }
    }

    public record DataResponse(string Id, string Type, string? Number, string Cvv, DateTime CreatedAt, DateTime UpdatedAt);
    public record Response(IEnumerable<DataResponse> cards, Pagination pagination);
}
