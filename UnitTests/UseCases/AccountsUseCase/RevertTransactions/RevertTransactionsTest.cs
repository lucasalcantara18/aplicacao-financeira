using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using Domain.Exceptions;
using Application.Services;
using Application.UseCases.AccountsUseCase.RevertTransaction;

namespace UnitTests.UseCases.AccountsUseCase.RevertTransactions
{
    public class RevertTransactionsTest
    {
        private readonly Mock<IContaRepository> _contaRepository;
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IRevertTransactionUseCase _useCase;

        public RevertTransactionsTest()
        {
            _contaRepository = new Mock<IContaRepository>();
            _transactionRepository = new Mock<ITransactionRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();
            _useCase = new RevertTransactionUseCase(_unitOfWork.Object, _transactionRepository.Object);
        }

        [Fact]
        public async Task RevertTransaction_Simples_Sucesso()
        {
            // Arrange
            var id = "1324";
            var cobrancaOriginal = new ClientTransaction
            {
                Id = id.ToString(),
                Value = 5M,
                Description = "description",
                CreatedAt = DateTime.Now.AddDays(-1),
                UpdatedAt = DateTime.Now.AddDays(-1),
                ContaId = "1",
                IsReverted = false,
                Internal = string.Empty,
                Conta = new Conta 
                { 
                    Transactions = new List<ClientTransaction>()
                    {
                        new ClientTransaction(10M, "description", "1"),
                        new ClientTransaction()
                        {
                            Id = id.ToString(),
                            Value = 5M,
                            Description = "description",
                            CreatedAt = DateTime.Now.AddDays(-1),
                            UpdatedAt = DateTime.Now.AddDays(-1),
                            ContaId = "1",
                            IsReverted = false,
                            Internal = string.Empty,
                        }
                    }
                }
            };

            _transactionRepository.Setup(r => r.SingleAsync(x => x.Id == "1324", default))
                .ReturnsAsync(cobrancaOriginal);

            _unitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var response = await _useCase.HandleAsync("1", id.ToString());

            // Assert

            Assert.NotNull(response);
            Assert.Equal(10M, cobrancaOriginal.Conta.Transactions.Sum(t => t.Value));

        }

        [Fact]
        public async Task RevertTransaction_Simples_Excpetion_Saldo_Insuficiente()
        {
            // Arrange
            var id = "1324";
            var cobrancaOriginal = new ClientTransaction
            {
                Id = id.ToString(),
                Value = 20M,
                Description = "description",
                CreatedAt = DateTime.Now.AddDays(-1),
                UpdatedAt = DateTime.Now.AddDays(-1),
                ContaId = "1",
                IsReverted = false,
                Internal = string.Empty,
                Conta = new Conta
                {
                    Id = "1",
                    Transactions = new List<ClientTransaction>()
                    {
                        new ClientTransaction(-10M, "description", "1"),
                        new ClientTransaction()
                        {
                            Id = id.ToString(),
                            Value = 20M,
                            Description = "description",
                            CreatedAt = DateTime.Now.AddDays(-1),
                            UpdatedAt = DateTime.Now.AddDays(-1),
                            ContaId = "1",
                            IsReverted = false,
                            Internal = string.Empty,
                        }
                    }
                }
            };

            _transactionRepository.Setup(r => r.SingleAsync(x => x.Id == "1324", default))
                .ReturnsAsync(cobrancaOriginal);

            _unitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);


            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("1", "1324"));

            Assert.Equal("Saldo insuficiente para estorno da transação.", ex.Message);
        }

        [Fact]
        public async Task AddInternalTransactions_Excpetion_Valor_negativo()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("1", "2"));

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
                _useCase.HandleAsync("1", "2"));

            Assert.Equal("Conta 2 não encontrado.", ex.Message);
        }
    }
}
