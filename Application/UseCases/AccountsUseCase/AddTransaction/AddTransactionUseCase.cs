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

namespace Application.UseCases.AccountsUseCase.AddTransaction
{
    public class AddTransactionUseCase : IAddTransactionUseCase
    {
        private readonly IContaRepository _contaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddTransactionUseCase(IContaRepository contaRepository, IUnitOfWork unitOfWork)
        {
            _contaRepository = contaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> HandleAsync(decimal value, string description, string accountId)
        {
            var conta = await _contaRepository.SingleAsync(x => x.Id == accountId);

            if(conta == null)
                throw new ApiException("Conta não encontrado.");

            var balance = conta.Transactions.Sum(t => t.Value);

            if ((value < 0 && balance <= 0) || ((balance + value) < 0))
                throw new ApiException("Saldo insuficiente para realizar a transação.");


            var newTransaction = new ClientTransaction(value, description, accountId);

            conta.Transactions.Add(newTransaction);

            await _unitOfWork.SaveAsync();

            return new Response(
                newTransaction.Id,
                newTransaction.Value,
                newTransaction.Description,
                newTransaction.CreatedAt,
                newTransaction.UpdatedAt
            );
        }

    }

    public record Response(string Id, decimal Value, string Description, DateTime CreatedAt, DateTime UpdatedAt);
}
