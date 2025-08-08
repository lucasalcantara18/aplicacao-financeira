using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.GetAccounts
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetAccountsUseCase _getAccountsUseCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IGetAccountsUseCase getAccountsUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _getAccountsUseCase = getAccountsUseCase;
        }

        [HttpGet(Name = "account")]
        [Authorize]
        public async Task<IActionResult> GetAccounts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");

            var response = await _getAccountsUseCase.HandleAsync(userId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
