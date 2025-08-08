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

namespace Application.UseCases.AccountsUseCase.GetAccounts
{
    public class GetAccountsUseCase : IGetAccountsUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        public GetAccountsUseCase(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }
        public async Task<List<Response>> HandleAsync(string clientId)
        {
            var pessoa = await _pessoaRepository.SingleAsync(x => x.Id == clientId);

            if(pessoa == null)
                throw new ApiException("Cliente não encontrado.");

            var response = pessoa.Contas.Select(x =>
            {
                return new Response(
                    x.Id,
                    x.Branch,
                    x.Account,
                    x.CreatedAt,
                    x.UpdatedAt
                );
            });


            return response.ToList();
        }
    }

    public record Response(string Id, string Branch, string Account, DateTime CreatedAt, DateTime UpdatedAt);
}
