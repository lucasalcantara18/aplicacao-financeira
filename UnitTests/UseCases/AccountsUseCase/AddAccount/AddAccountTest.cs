using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Application.Services;
using Application.UseCases.AccountsUseCase.AddAccount;

namespace UnitTests.UseCases.AccountsUseCase.AddAccount
{
    public class AddAccountTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IAddAccountUseCase _useCase;

        public AddAccountTest()
        {
            _pessoaRepository = new Mock<IPessoaRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _useCase = new AddAccountUseCase(_pessoaRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task AddAccount_Sucesso()
        {
            // Arrange
            var cliente = new Pessoa
            {
                Id = "1",
                Contas = new List<Conta>()
            };

            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(cliente);

            _unitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var response = await _useCase.HandleAsync("123", "1234567-0", "1");

            // Assert
            Assert.NotNull(response);
            Assert.Equal("123", response.Branch);
            Assert.Equal("1234567-0", response.Account);
            Assert.Equal(1, cliente.Contas.Count);
            Assert.Equal(response.Id, cliente.Contas[0].Id);
            Assert.Equal(cliente.Contas[0].CreatedAt, response.CreatedAt);
        }

        [Fact]
        public async Task AddAccount_Exception_Nao_Encontrado()
        {
            // Arrange
            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync((Pessoa?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("123", "1234567-0", "client-id"));

            Assert.Equal("Cliente não encontrado.", ex.Message);
        }

        [Fact]
        public async Task AddAccount_Exception_Branch()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("12", "1234567-0", "client-id"));

            Assert.Equal("Agência inválida. Deve conter 3 dígitos numéricos.", ex.Message);
        }

        [Fact]
        public async Task AddAccount_Exception_Account()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("123", "1234567-04", "client-id"));

            Assert.Equal("Conta inválida. Deve estar no formato 0000000-0.", ex.Message);
        }
    }
}
