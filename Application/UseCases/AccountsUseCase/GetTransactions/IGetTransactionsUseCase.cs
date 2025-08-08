using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.GetTransactions
{
    public interface IGetTransactionsUseCase
    {
        Task<Response> HandleAsync(int itemsPerPage, int currentPage, string type, string accountId);
    }
}
