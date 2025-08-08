using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using System.Linq;
using Application.UseCases.AccountsUseCase.GetBalance;
using Application.UseCases.AccountsUseCase.GetTransactions;

namespace UnitTests.UseCases.AccountsUseCase.GetTransactions
{
    public class GetTransactionsTest
    {
        private readonly Mock<IContaRepository> _contaRepository;
        private readonly IGetTransactionsUseCase _useCase;

        public GetTransactionsTest()
        {
            _contaRepository = new Mock<IContaRepository>();
            _useCase = new GetTransactionsUseCase(_contaRepository.Object);
        }

        [Fact]
        public async Task GetTransactions_Sucesso()
        {
            // Arrange
            var conta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 100, CreatedAt = DateTime.Now.AddDays(-1) },
                    new ClientTransaction { Value = -50, CreatedAt = DateTime.Now.AddDays(-2) },
                    new ClientTransaction { Value = 1500, CreatedAt = DateTime.Now.AddDays(-3) },
                    new ClientTransaction { Value = -800, CreatedAt = DateTime.Now.AddDays(-4) }
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync(conta);

            // Act
            var response = await _useCase.HandleAsync(10, 1, "credit", "1");

            // Assert

            Assert.NotNull(response);
            Assert.Equal(2, response.Transactions.Count());
        }

        [Fact]
        public async Task GetTransactions_Exception_Conta_Nao_Encontrado()
        {
            // Arrange
            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync((Conta?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(10, 1, "credit", "1"));

            Assert.Equal("Conta não encontrado.", ex.Message);
        }
    }
}
