using Application.Services;
using Application.Services.External.ComplianceService;
using Application.UseCases.PeopleUseCase.AddPeople;
using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Application.UseCases.LoginUseCase.SignIn;

namespace UnitTests.UseCases.LoginUseCase
{
    public class SignInTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly IConfiguration _configuration;
        private readonly ISignInUseCase _useCase;

        public SignInTest()
        {
            var inMemorySettings = new Dictionary<string, string>
                {
                    {"Jwt:Key", "2b48f1976502215b00529001b864c7ac5ac53c5ce974744f8401c2e8788b60f6f698e338"},
                    {"Jwt:Issuer", "TestIssuer"},
                    {"Jwt:Audience", "TestAudience"},
                    {"Jwt:ExpireMinutes", "60"}
                };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _pessoaRepository = new Mock<IPessoaRepository>();
            _useCase = new SignInUseCase(_pessoaRepository.Object, _configuration);
        }

        [Fact]
        public async Task SignIn_Sucesso()
        {
            // Arrange
            var pessoa = new Pessoa("Alguem", "senha123", "12345678900");

            _pessoaRepository
                .Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(pessoa);

            // Act
            var result = await _useCase.HandleAsync("12345678900", "senha123");

            // Assert
            Assert.StartsWith("Bearer ", result);
            Assert.True(result.Length > "Bearer ".Length);
        }

        [Fact]
        public async Task SignIn_Erro_Pessoa_Nao_Encontrada()
        {
            // Arrange
            _pessoaRepository
                .Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync((Pessoa?)null);

            // Act
            var result = await _useCase.HandleAsync("12345678900", "senha");

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
