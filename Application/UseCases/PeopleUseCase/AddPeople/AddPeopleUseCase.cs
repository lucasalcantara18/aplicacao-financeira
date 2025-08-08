using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Application.Services;
using Application.Services.External.ComplianceService;
using Domain.Exceptions;

namespace Application.UseCases.PeopleUseCase.AddPeople
{
    public class AddPeopleUseCase : IAddPeopleUseCase
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IComplianceService _complianceService;
        public AddPeopleUseCase(IPessoaRepository pessoaRepository, IComplianceService complianceService, IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _complianceService = complianceService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> HandleAsync(string name, string document, string password)
        {
            var documentFormated = document.Replace(".", "").Replace("-", "").Replace("/", "").Trim();

            var pessoa = await _pessoaRepository.SingleAsync(x => x.Documento == documentFormated);
            if (pessoa != null)
                throw new ApiException("Pessoa já cadastrada com este documento");

            var isValidDocument = await _complianceService.CpfCnpjValidatorAsync(documentFormated);

            if (isValidDocument)
            {
                var newPessoa = new Pessoa(name, password, documentFormated);

                await _pessoaRepository.AddAsync(newPessoa);
                await _unitOfWork.SaveAsync();

                return new Response(newPessoa.Id, newPessoa.Name, newPessoa.Documento, newPessoa.CreatedAt, newPessoa.UpdatedAt);
            }

            throw new ApiException("Documento inválido");

        }
    }

    public record Response(string Id, string Name, string Document, DateTime CreatedAt, DateTime UpdatedAt);
}
