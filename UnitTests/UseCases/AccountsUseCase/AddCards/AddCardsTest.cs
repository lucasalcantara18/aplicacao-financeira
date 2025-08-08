using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Application.Services;
using Application.UseCases.AccountsUseCase.AddCards;

namespace UnitTests.UseCases.AccountsUseCase.AddCards
{
    public class AddCardsTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IAddCardUseCase _useCase;

        public AddCardsTest()
        {
            _pessoaRepository = new Mock<IPessoaRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _useCase = new AddCardUseCase(_pessoaRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task AddAccount_Sucesso()
        {
            // Arrange
            var cliente = new Pessoa
            {
                Id = "1",
                Contas = new List<Conta>() { new Conta() { Id = "1" } }
            };

            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(cliente);

            _unitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var response = await _useCase.HandleAsync("physical", "1111 1111 1111 1111", "123", "1", "1");

            // Assert
            Assert.NotNull(response);
            Assert.Equal("1111", response.Number);
            Assert.Equal("123", response.Cvv);
            Assert.Equal(1, cliente.Contas.FirstOrDefault().Cartoes.Count);
        }

        [Fact]
        public async Task AddAccount_Exception_Pessoa_Nao_Encontrado()
        {
            // Arrange
            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync((Pessoa?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("physical", "1111 1111 1111 1111", "123", "1", "1"));

            Assert.Equal("Cliente não encontrado.", ex.Message);
        }

        [Fact]
        public async Task AddAccount_Exception_Conta_Nao_Encontrado()
        {
            // Arrange
            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(new Pessoa { Id = "1", Contas = [] });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("physical", "1111 1111 1111 1111", "123", "1", "1"));

            Assert.Equal("Conta não encontrada.", ex.Message);
        }

        [Fact]
        public async Task AddCard_Exception_Cvv()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("physical", "1111 1111 1111 1111", "12", "1", "1"));

            Assert.Equal("Cvv inválida. Deve conter 3 dígitos numéricos.", ex.Message);
        }

        [Fact]
        public async Task AddCard_Exception_Number()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("physical", "1111 1111 1111 11111", "123", "1", "1"));

            Assert.Equal("Numero inválido. Deve estar no formato 0000 0000 00000 00000.", ex.Message);
        }

        [Fact]
        public async Task AddCard_Exception_Type()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("teste", "1111 1111 1111 1111", "123", "1", "1"));

            Assert.Equal("Tipo de cartão inválido. Deve ser 'physical' ou 'virtual'.", ex.Message);
        }
    }
}
