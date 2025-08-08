using Application.UseCases.LoginUseCase.SignIn;
using Application.UseCases.PeopleUseCase.AddPeople;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppCubos.Controllers.Login;

namespace WebAppCubos.Controllers.People.AddPeople
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddPeopleUseCase _addPeopleUseCase;

        public PeopleController(ILogger<PeopleController> logger, IConfiguration configuration, IAddPeopleUseCase addPeopleUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _addPeopleUseCase = addPeopleUseCase;
        }

        [HttpPost(Name = "people")]
        [EncryptPassword]
        public async Task<IActionResult> LoginAsync([FromBody] PeopleRequest request)
        {
            var response = await _addPeopleUseCase.HandleAsync(request.Name, request.Document, request.Password);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
