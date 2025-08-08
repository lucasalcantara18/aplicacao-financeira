using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.AddAccount
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddAccountUseCase _addAccountUseCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IAddAccountUseCase addAccountUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _addAccountUseCase = addAccountUseCase;
        }

        [HttpPost(Name = "account")]
        [Authorize]
        public async Task<IActionResult> LoginAsync([FromBody] AccountRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");

            var response = await _addAccountUseCase.HandleAsync(request.Branch, request.Account, userId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
