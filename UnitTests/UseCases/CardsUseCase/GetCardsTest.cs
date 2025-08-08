using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Application.UseCases.CardsUseCase.GetCards;
using Domain.Exceptions;

namespace UnitTests.UseCases.CardsUseCase
{
    public class GetCardsTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly IGetUserCardsUseCase _useCase;

        public GetCardsTest()
        {
            _pessoaRepository = new Mock<IPessoaRepository>();
            _useCase = new GetUserCardsUseCase(_pessoaRepository.Object);
        }

        [Fact]
        public async Task GetCards_Sucesso_paginado()
        {
            // Arrange
            var dataCriacao = DateTime.UtcNow;

            var cartoes = new List<Cartao>
            {
                new Cartao
                {
                    Id = "1", Type = "Credit", Number = "1234 5678 9012 3456", Cvv = "123",
                    CreatedAt = dataCriacao.AddMinutes(-2), UpdatedAt = dataCriacao
                },
                new Cartao
                {
                    Id = "2", Type = "Debit", Number = "9876 5432 1098 7654", Cvv = "456",
                    CreatedAt = dataCriacao.AddMinutes(-1), UpdatedAt = dataCriacao
                },
                new Cartao
                {
                    Id = "3", Type = "Credit", Number = "1111 2222 3333 4444", Cvv = "789",
                    CreatedAt = dataCriacao.AddMinutes(-3), UpdatedAt = dataCriacao
                },
                new Cartao
                {
                    Id = "4", Type = "Credit", Number = "1111 2222 3333 4444", Cvv = "789",
                    CreatedAt = dataCriacao.AddMinutes(-4), UpdatedAt = dataCriacao
                }
            };

            var pessoa = new Pessoa
            {
                Id = "1",
                CreatedAt = dataCriacao,
                UpdatedAt = dataCriacao,
                Name = "Cliente",
                Contas = new List<Conta>
                {
                    new Conta { Cartoes = cartoes, CreatedAt = dataCriacao, UpdatedAt = dataCriacao, Id = "1" }
                }
            };

            _pessoaRepository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(pessoa);

            // Act
            var result = await _useCase.HandleAsync(2, 1, "1");

            // Assert
            Assert.NotNull(result);
            var lista = result!.cards.ToList();

            Assert.Equal(2, lista.Count);
            Assert.Equal("2", lista[0].Id); // Cartão com data mais recente primeiro
            Assert.Equal("1", lista[1].Id);

            Assert.Equal(4, result.pagination.TotalItems);
            Assert.Equal(1, result.pagination.CurrentPage);
            Assert.Equal(2, result.pagination.ItemsPerPage);
        }

        [Fact]
        public async Task GetCards_Exception()
        {
            // Arrange
            _pessoaRepository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync((Pessoa?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() =>
                _useCase.HandleAsync(10, 1, "1"));

            Assert.Equal("Cliente não encontrado.", ex.Message);
        }
    }
}
