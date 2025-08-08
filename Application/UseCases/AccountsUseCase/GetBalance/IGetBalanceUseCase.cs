using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.GetBalance
{
    public interface IGetBalanceUseCase
    {
        Task<Response> HandleAsync(string accountId);
    }
}
