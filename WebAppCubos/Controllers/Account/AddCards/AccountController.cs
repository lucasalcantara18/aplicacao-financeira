using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.AddCards
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddCardUseCase _addCardUseCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IAddCardUseCase addCardUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _addCardUseCase = addCardUseCase;
        }

        [HttpPost("{accountId}/cards")]
        [Authorize]
        public async Task<IActionResult> LoginAsync([FromRoute] string accountId, [FromBody] AddCardRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");

            var response = await _addCardUseCase.HandleAsync(request.Type, request.Number, request.Cvv, accountId, userId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
