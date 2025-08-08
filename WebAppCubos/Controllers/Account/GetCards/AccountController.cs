using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.GetCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.GetCards
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetCardsUseCase _getCardsUseCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IGetCardsUseCase getCardsUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _getCardsUseCase = getCardsUseCase;
        }

        [HttpGet("{accountId}/cards")]
        [Authorize]
        public async Task<IActionResult> GetCards([FromRoute] string accountId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");

            var response = await _getCardsUseCase.HandleAsync(userId, accountId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
