using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.AddTransaction;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.AddTransaction
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddTransactionUseCase _useCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IAddTransactionUseCase useCase)
        {
            _logger = logger;
            _configuration = configuration;
            _useCase = useCase;
        }

        [HttpPost("{accountId}/transactions")]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromRoute] string accountId, [FromBody] AddTransactionRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");
                
            var response = await _useCase.HandleAsync(request.Value, request.Description, accountId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
