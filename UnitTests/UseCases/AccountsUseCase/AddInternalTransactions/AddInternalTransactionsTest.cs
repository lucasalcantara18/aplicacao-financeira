using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Application.Services;
using Application.UseCases.AccountsUseCase.AddTransaction;
using Application.UseCases.AccountsUseCase.AddInternalTransaction;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTests.UseCases.AccountsUseCase.AddInternalTransactions
{
    public class AddInternalTransactionsTest
    {
        private readonly Mock<IContaRepository> _contaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IAddInternalTransactionUseCase _useCase;

        public AddInternalTransactionsTest()
        {
            _contaRepository = new Mock<IContaRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _useCase = new AddInternalTransactionUseCase(_contaRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task AddInternalTransactions_Sucesso()
        {
            // Arrange
            var originalConta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 100, CreatedAt = DateTime.Now.AddDays(-1) },
                },
            };

            var targetConta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(x => x.Id == "1", default))
                .ReturnsAsync(originalConta);

            _contaRepository.Setup(r => r.SingleAsync(x => x.Id == "2", default))
                .ReturnsAsync(targetConta);

            _unitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var response = await _useCase.HandleAsync(10M, "Description", "1", "2");

            // Assert

            Assert.NotNull(response);
            Assert.Equal(10M, response.Value);
            Assert.Equal(90M, originalConta.Transactions.Sum(t => t.Value));
            Assert.Equal(10M, targetConta.Transactions.Sum(t => t.Value));

        }

        [Fact]
        public async Task AddInternalTransactions_Excpetion_Saldo_Insuficiente()
        {
            // Arrange
            var originalConta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 5, CreatedAt = DateTime.Now.AddDays(-1) },
                },
            };

            var targetConta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(x => x.Id == "1", default))
                .ReturnsAsync(originalConta);

            _contaRepository.Setup(r => r.SingleAsync(x => x.Id == "2", default))
                .ReturnsAsync(targetConta);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(20M, "Description", "1", "2"));

            Assert.Equal("Saldo insuficiente para realizar a transação.", ex.Message);
        }

        [Fact]
        public async Task AddInternalTransactions_Excpetion_Valor_negativo()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(-20M, "Description", "1", "2"));

            Assert.Equal("Apenas operações de debito em transação interna.", ex.Message);
        }

        [Fact]
        public async Task AddInternalTransactions_Exception_Conta_Nao_Encontrado()
        {
            // Arrange
            var originalConta = new Conta
            {
                Id = "1",
                PessoaId = "123",
                Transactions = new List<ClientTransaction>
                {
                    new ClientTransaction { Value = 5, CreatedAt = DateTime.Now.AddDays(-1) },
                },
            };

            _contaRepository.Setup(r => r.SingleAsync(x => x.Id == "1", default))
                .ReturnsAsync(originalConta);

            _contaRepository.Setup(r => r.SingleAsync(x => x.Id == "2", default))
                .ReturnsAsync((Conta)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(10M, "Description", "1", "2"));

            Assert.Equal("Conta 2 não encontrado.", ex.Message);
        }
    }
}
