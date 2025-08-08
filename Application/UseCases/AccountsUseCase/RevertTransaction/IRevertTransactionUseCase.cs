using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AccountsUseCase.RevertTransaction
{
    public interface IRevertTransactionUseCase
    {
        Task<object> HandleAsync(string accountId, string transactionId);
    }
}
