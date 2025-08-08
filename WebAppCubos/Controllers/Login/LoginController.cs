using Application.UseCases.LoginUseCase.SignIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAppCubos.Controllers.Login
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISignInUseCase _signInUseCase;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, ISignInUseCase signInUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _signInUseCase = signInUseCase;
        }

        [HttpPost(Name = "login")]
        [EncryptPassword]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var token = await _signInUseCase.HandleAsync(request.Document, request.Password);

            if(!string.IsNullOrEmpty(token))
                return Ok(new { token });

            return Unauthorized();
        }
    }
}
