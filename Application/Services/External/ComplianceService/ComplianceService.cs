using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Application.Services;
using Application.Services.External.Dtos;
using Application.Services.External.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Domain.Exceptions;

namespace Application.Services.External.ComplianceService
{
    public class ComplianceService : IComplianceService
    {
        private readonly IExternalApi _externalApi;
        private readonly IConfiguration _configuration;

        public ComplianceService(IExternalApi externalApi, IConfiguration configuration)
        {
            _externalApi = externalApi;
            _configuration = configuration;
        }

        private async Task<string> AuthAsync()
        {
            var complianceCredencials = _configuration.GetSection("ComplianceCredencials");
            var request = new ExternalCodeRequest { Email = complianceCredencials["email"], Password = complianceCredencials["password"] };

            var responseCode = await _externalApi.RequestAuthCodeAsync(request);

            if (responseCode == null || !responseCode.Success)
            {
                throw new ApiException("Erro ao obter código de autenticação do serviço de compliance.");
            }

            var code = JsonConvert.DeserializeObject<ExternalCodeResponse>(responseCode.Data.ToString());

            var responseToken = await _externalApi.RequestAccessTokenAsync(new ExternalTokenRequest { AuthCode = code.AuthCode.ToString() });

            if (responseToken == null || !responseToken.Success)
            {
                throw new ApiException("Erro ao obter token de acesso do serviço de compliance.");
            }

            var token = JsonConvert.DeserializeObject<ExternalTokenResponse>(responseToken.Data.ToString());
            return token.AccessToken;
        }

        public async Task<bool> CpfCnpjValidatorAsync(string documento)
        {
            var token = await AuthAsync();
            ExternalApiResponse response = null;
            if (documento.Length == 11)
                response = await _externalApi.ValidateCpfAsync(new ExternalDocumentRequest { Document = documento }, $"Bearer {token}");
            else
                response = await _externalApi.ValidateCnpjAsync(new ExternalDocumentRequest { Document = documento }, $"Bearer {token}");

            var status = JsonConvert.DeserializeObject<ExternalDocumentResponse>(response.Data.ToString());

            return response != null && response.Success
                ? status.Status
                : throw new ApiException("Erro ao validar CPF/CNPJ no serviço de compliance.");
        }
    }
}
