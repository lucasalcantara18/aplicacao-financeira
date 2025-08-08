using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.LoginUseCase.SignIn
{
    public interface ISignInUseCase
    {
        Task<string> HandleAsync(string document, string password);
    }
}
