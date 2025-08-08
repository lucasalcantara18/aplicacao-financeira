using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using System.Linq;
using Application.UseCases.AccountsUseCase.GetBalance;
using Application.UseCases.AccountsUseCase.GetTransactions;
using Application.Services;
using Application.UseCases.AccountsUseCase.AddTransaction;

namespace UnitTests.UseCases.AccountsUseCase.AddTransactions
{
    public class AddTransactionsTest
    {
        private readonly Mock<IContaRepository> _contaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IAddTransactionUseCase _useCase;

        public AddTransactionsTest()
        {
            _contaRepository = new Mock<IContaRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _useCase = new AddTransactionUseCase(_contaRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task AddTransactions_Sucesso()
        {
            // Arrange
            var conta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 100, CreatedAt = DateTime.Now.AddDays(-1) },
                    new ClientTransaction { Value = -50, CreatedAt = DateTime.Now.AddDays(-2) }
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync(conta);
            _unitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var response = await _useCase.HandleAsync(10M, "Description", "1");

            // Assert

            Assert.NotNull(response);
            Assert.Equal(10M, response.Value); 
            Assert.Equal(60M, conta.Transactions.Sum(t => t.Value));
            
        }

        [Fact]
        public async Task AddTransactions_Excpetion_Saldo_Insuficiente()
        {
            // Arrange
            var conta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 100, CreatedAt = DateTime.Now.AddDays(-1) },
                    new ClientTransaction { Value = -50, CreatedAt = DateTime.Now.AddDays(-2) }
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync(conta);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(-60M, "Description", "1"));

            Assert.Equal("Saldo insuficiente para realizar a transação.", ex.Message);
        }

        [Fact]
        public async Task AddTransactions_Exception_Conta_Nao_Encontrado()
        {
            // Arrange
            _contaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Conta, bool>>>(), default))
                .ReturnsAsync((Conta?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(10M, "Description", "1"));

            Assert.Equal("Conta não encontrado.", ex.Message);
        }
    }
}
