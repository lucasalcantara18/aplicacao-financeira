using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.GetCards
{
    public interface IGetCardsUseCase
    {
        Task<List<Response>> HandleAsync(string clientId, string accountId);
    }
}
