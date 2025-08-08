using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.UseCases.LoginUseCase.SignIn
{
    public class SignInUseCase : ISignInUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IConfiguration _configuration;
        public SignInUseCase(IPessoaRepository pessoaRepository, IConfiguration configuration)
        {
            _pessoaRepository = pessoaRepository;
            _configuration = configuration;
        }
        public async Task<string> HandleAsync(string document, string password)
        {
            var pessoa = await _pessoaRepository.SingleAsync(x => x.Documento == document);
            if (pessoa == null)
                return string.Empty;

            if (pessoa.Password == password)
            {
                var userId = pessoa.Id; // Simule um ID real do banco

                var token = GenerateJwtToken(userId);
                return $"Bearer {token}";
            }

            return string.Empty;
        }

        private string GenerateJwtToken(string userId)
        {
            var jwt = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"])),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
