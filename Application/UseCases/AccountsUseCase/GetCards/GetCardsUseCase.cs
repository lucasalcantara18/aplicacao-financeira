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

namespace Application.UseCases.AccountsUseCase.GetCards
{
    public class GetCardsUseCase : IGetCardsUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        public GetCardsUseCase(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }
        public async Task<List<Response>> HandleAsync(string clientId, string accountIdn)
        {
            var pessoa = await _pessoaRepository.SingleAsync(x => x.Id == clientId);

            if(pessoa == null)
                throw new ApiException("Cliente não encontrado.");

            if(!pessoa.Contas.Any(x => x.Id == accountIdn))
                throw new ApiException("Conta não encontrada.");

            var response = pessoa.Contas.FirstOrDefault(x => x.Id == accountIdn).Cartoes.Select(x =>
            {
                return new Response(
                    x.Id,
                    x.Type,
                    x.Number,
                    x.Cvv,
                    x.CreatedAt,
                    x.UpdatedAt
                );
            });

            return response.ToList();
        }

    }

    public record Response(string Id, string Type, string Number, string Cvv, DateTime CreatedAt, DateTime UpdatedAt);
}
