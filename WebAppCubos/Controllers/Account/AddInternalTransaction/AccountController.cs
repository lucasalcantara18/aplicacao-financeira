using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.AddInternalTransaction;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.AddInternalTransaction
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddInternalTransactionUseCase _useCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IAddInternalTransactionUseCase useCase)
        {
            _logger = logger;
            _configuration = configuration;
            _useCase = useCase;
        }

        [HttpPost("{accountId}/transactions/internal")]
        [Authorize]
        public async Task<IActionResult> LoginAsync([FromRoute] string accountId, [FromBody] AddInternalTransactionRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");

            var response = await _useCase.HandleAsync(request.Value, request.Description, accountId, request.ReceiverAccountId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
