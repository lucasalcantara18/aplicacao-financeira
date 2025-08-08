using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Application.Services;
using Application.UseCases.AccountsUseCase.GetAccounts;
using System.Linq;

namespace UnitTests.UseCases.AccountsUseCase.GetAccounts
{
    public class GetAccountsTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly IGetAccountsUseCase _useCase;

        public GetAccountsTest()
        {
            _pessoaRepository = new Mock<IPessoaRepository>();
            _useCase = new GetAccountsUseCase(_pessoaRepository.Object);
        }

        [Fact]
        public async Task AddAccount_Sucesso()
        {
            // Arrange
            var cliente = new Pessoa
            {
                Id = "1",
                Contas = new List<Conta>() { new Conta() { Id = "1", Account = "2033392-6", Branch = "111", Cartoes = [], CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow} }
            };

            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(cliente);

            // Act
            var response = await _useCase.HandleAsync("1");

            // Assert

            Assert.NotNull(response);
            Assert.Equal("2033392-6", response.FirstOrDefault().Account);
            Assert.Equal("111", response.FirstOrDefault().Branch);
        }

        [Fact]
        public async Task AddAccount_Exception_Pessoa_Nao_Encontrado()
        {
            // Arrange
            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync((Pessoa?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("1"));

            Assert.Equal("Cliente não encontrado.", ex.Message);
        }
    }
}
