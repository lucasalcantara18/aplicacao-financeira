using Application.Services.External.Dtos;
using Refit;

namespace Application.Services.External.Core
{
    public interface IExternalApi
    {
        [Post("/auth/code")]
        Task<ExternalApiResponse> RequestAuthCodeAsync([Body] ExternalCodeRequest body);

        [Post("/auth/token")]
        Task<ExternalApiResponse> RequestAccessTokenAsync([Body] ExternalTokenRequest body);

        [Post("/cpf/validate")]
        Task<ExternalApiResponse> ValidateCpfAsync([Body] ExternalDocumentRequest body, [Header("Authorization")] string token);

        [Post("/cnpj/validate")]
        Task<ExternalApiResponse> ValidateCnpjAsync([Body] ExternalDocumentRequest body, [Header("Authorization")] string token);
    }
}
