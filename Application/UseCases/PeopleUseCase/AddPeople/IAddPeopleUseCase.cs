using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.PeopleUseCase.AddPeople
{
    public interface IAddPeopleUseCase
    {
        Task<Response> HandleAsync(string name, string document, string password);
    }
}
