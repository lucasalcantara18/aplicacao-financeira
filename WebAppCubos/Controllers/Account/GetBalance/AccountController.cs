using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.AddTransaction;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Application.UseCases.AccountsUseCase.GetBalance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.GetBalance
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetBalanceUseCase _useCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IGetBalanceUseCase useCase)
        {
            _logger = logger;
            _configuration = configuration;
            _useCase = useCase;
        }

        [HttpGet("{accountId}/balance")]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromRoute] string accountId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");
                
            var response = await _useCase.HandleAsync(accountId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
