using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Application.UseCases.AccountsUseCase.GetCards;

namespace UnitTests.UseCases.AccountsUseCase.GetCards
{
    public class GetCardsTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly IGetCardsUseCase _useCase;

        public GetCardsTest()
        {
            _pessoaRepository = new Mock<IPessoaRepository>();
            _useCase = new GetCardsUseCase(_pessoaRepository.Object);
        }

        [Fact]
        public async Task GetCards_Sucesso()
        {
            // Arrange
            var cliente = new Pessoa
            {
                Id = "1",
                Contas = new List<Conta>() { new Conta() { Id = "1", Account = "2033392-6", Branch = "111", Cartoes = new List<Cartao>() { new Cartao { ContaId = "1", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Cvv = "111", Number = "1111 1111 1111 1111", Id = "1", Type = "physical" } }, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow} }
            };

            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(cliente);

            // Act
            var response = await _useCase.HandleAsync("1", "1");

            // Assert

            Assert.NotNull(response);
            Assert.Equal("111", response.FirstOrDefault().Cvv);
            Assert.Equal("1111 1111 1111 1111", response.FirstOrDefault().Number);
        }

        [Fact]
        public async Task GetCards_Exception_Pessoa_Nao_Encontrado()
        {
            // Arrange
            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync((Pessoa?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync("1", "1"));

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
                _useCase.HandleAsync("1", "1"));

            Assert.Equal("Conta não encontrada.", ex.Message);
        }
    }
}
