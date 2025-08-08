using Application.Services;
using Application.Services.External.ComplianceService;
using Application.UseCases.PeopleUseCase.AddPeople;
using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Moq;
using System.Linq.Expressions;
using Domain.Exceptions;

namespace UnitTests.UseCases.PeopleUseCase.AddPeople
{
    public class AddPeopleTest
    {
        private readonly Mock<IPessoaRepository> _pessoaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IComplianceService> _complianceService;
        private readonly IAddPeopleUseCase _useCase;

        public AddPeopleTest()
        {
            _pessoaRepository = new Mock<IPessoaRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _complianceService = new Mock<IComplianceService>();
            _useCase = new AddPeopleUseCase(_pessoaRepository.Object, _complianceService.Object, _unitOfWork.Object);
        }

        [Fact]
        public void Add_people_success()
        {
            _complianceService.Setup(x => x.CpfCnpjValidatorAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _pessoaRepository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default)).ReturnsAsync(() => null);
            _pessoaRepository.Setup(x => x.AddAsync(It.IsAny<Pessoa>(), default));


            _unitOfWork.Setup(x => x.SaveAsync())
                .ReturnsAsync(1);
            
            // Act
            var result = _useCase.HandleAsync("Alguem", "111.222.333-45", "pass").Result;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alguem", result.Name);
            Assert.DoesNotContain("-", result.Document);
            _pessoaRepository.Verify(x => x.AddAsync(It.IsAny<Pessoa>(), default), Times.Once);
            _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Fact]
        public async Task Add_people_Exception()
        {
            // Arrange
            string doc = "111.222.333-45";
            string docFormatado = "11122233345";

            _pessoaRepository.Setup(r => r.SingleAsync(It.IsAny<Expression<Func<Pessoa, bool>>>(), default))
                .ReturnsAsync(new Pessoa("Alguem", "pass", docFormatado));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() => _useCase.HandleAsync("Alguem", doc, "pass"));

            Assert.Equal("Pessoa já cadastrada com este documento", ex.Message);
        }
    }
}
