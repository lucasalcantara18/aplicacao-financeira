using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.AddCards
{
    public interface IAddCardUseCase
    {
        Task<Response> HandleAsync(string type, string number, string cvv, string accountId, string clientId);
    }
}
