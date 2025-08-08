using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.CardsUseCase.GetCards
{
    public interface IGetUserCardsUseCase
    {
        Task<Response> HandleAsync(int itemsPerPage, int currentPage, string clientId);
    }
}
