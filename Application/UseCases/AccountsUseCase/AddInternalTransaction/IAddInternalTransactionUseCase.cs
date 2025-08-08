using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.AddInternalTransaction
{
    public interface IAddInternalTransactionUseCase
    {
        Task<Response> HandleAsync(decimal value, string description, string accountId, string receiverAccountId);
    }
}
