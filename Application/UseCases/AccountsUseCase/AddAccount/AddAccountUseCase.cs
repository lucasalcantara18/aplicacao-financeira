using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Application.Services;
using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Application.UseCases.AccountsUseCase.AddAccount
{
    public class AddAccountUseCase : IAddAccountUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddAccountUseCase(IPessoaRepository pessoaRepository, IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> HandleAsync(string branch, string account, string clientId)
        {
            if(branch.Length != 3 || !branch.All(char.IsDigit))
                throw new ApiException("Agência inválida. Deve conter 3 dígitos numéricos.");

            if (!Regex.IsMatch(account, @"^\d{7}-\d$"))
                throw new ApiException("Conta inválida. Deve estar no formato 0000000-0.");

            var pessoa = await _pessoaRepository.SingleAsync(x => x.Id == clientId);

            if(pessoa == null)
                throw new ApiException("Cliente não encontrado.");

            if (pessoa.Contas.Any(x => x.Account == account))
                throw new ApiException("Numero de conta já existente");

            var newAccount = new Conta(branch, account, clientId);

            pessoa.Contas.Add(newAccount);

            await _unitOfWork.SaveAsync();

            return new Response(
                newAccount.Id,
                newAccount.Branch,
                newAccount.Account,
                newAccount.CreatedAt,
                newAccount.UpdatedAt
            );
        }

    }
    public record Response(string Id, string Branch, string Account, DateTime CreatedAt, DateTime UpdatedAt);
}
