using Application.Services.External.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.External.ComplianceService
{
    public interface IComplianceService
    {
        Task<bool> CpfCnpjValidatorAsync(string documento);
    }
}
