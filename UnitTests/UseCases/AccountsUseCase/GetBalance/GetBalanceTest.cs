using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Application.Services;
using Application.UseCases.AccountsUseCase.GetAccounts;
using System.Linq;
using Application.UseCases.AccountsUseCase.GetBalance;

namespace UnitTests.UseCases.AccountsUseCase.GetBalance
{
    public class GetBalanceTest
    {
        private readonly Mock<IContaRepository> _contaRepository;
        private readonly IGetBalanceUseCase _useCase;

        public GetBalanceTest()
        {
            _contaRepository = new Mock<IContaRepository>();
            _useCase = new GetBalanceUseCase(_contaRepository.Object);
        }

        [Fact]
        public async Task GetBalance_Sucesso()
        {
            // Arrange
            var conta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 100, CreatedAt = DateTime.Now.AddDays(-1) },
                    new ClientTransaction { Value = -50, CreatedAt = DateTime.Now }
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync(conta);

            // Act
            var response = await _useCase.HandleAsync("1");

            // Assert

            Assert.NotNull(response);
            Assert.Equal(50M, response.Balance);
        }

        [Fact]
        public async Task AddAccount_Exception_Pessoa_Nao_Encontrado()
        {
            // Arrange
            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync((Conta?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("1"));

            Assert.Equal("Conta não encontrado.", ex.Message);
        }
    }
}
