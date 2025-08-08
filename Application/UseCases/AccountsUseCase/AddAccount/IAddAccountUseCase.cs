using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.AddAccount
{
    public interface IAddAccountUseCase
    {
        Task<Response> HandleAsync(string branch, string account, string clientId);
    }
}
