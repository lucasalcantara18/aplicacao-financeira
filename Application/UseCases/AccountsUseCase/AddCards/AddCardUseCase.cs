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

namespace Application.UseCases.AccountsUseCase.AddCards
{
    public class AddCardUseCase : IAddCardUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string[] _cardTypes = { "physical", "virtual"};
        public AddCardUseCase(IPessoaRepository pessoaRepository, IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> HandleAsync(string type, string number, string cvv, string accountId, string clientId)
        {
            if(cvv.Length != 3 || !cvv.All(char.IsDigit))
                throw new ApiException("Cvv inválida. Deve conter 3 dígitos numéricos.");

            if (!Regex.IsMatch(number, @"^\d{4} \d{4} \d{4} \d{4}$"))
                throw new ApiException("Numero inválido. Deve estar no formato 0000 0000 00000 00000.");

            if(!_cardTypes.Contains(type))
                throw new ApiException("Tipo de cartão inválido. Deve ser 'physical' ou 'virtual'.");

            var pessoa = await _pessoaRepository.SingleAsync(x => x.Id == clientId);

            if(pessoa == null)
                throw new ApiException("Cliente não encontrado.");

            var conta = pessoa.Contas.FirstOrDefault(c => c.Id == accountId);

            if (conta == null)
                throw new ApiException("Conta não encontrada.");

            var cartoes = conta.Cartoes.ToList();

            if (cartoes.Any())
            {
                var existingPhysicalCard = cartoes.Any(c => c.Type == "physical");
                if (type == "physical" && existingPhysicalCard)
                    throw new ApiException("Já existe um cartão físico cadastrado para esta conta.");
            }

            var newCartao = new Cartao(type, number, cvv, accountId);

            conta.Cartoes.Add(newCartao);

            await _unitOfWork.SaveAsync();

            return new Response(
                newCartao.Id,
                newCartao.Type,
                newCartao.Number.Split(" ").LastOrDefault(),
                newCartao.Cvv,
                newCartao.CreatedAt,
                newCartao.UpdatedAt
            );
        }

    }

    public record Response(string Id, string Type, string? Number, string Cvv, DateTime CreatedAt, DateTime UpdatedAt);
}
