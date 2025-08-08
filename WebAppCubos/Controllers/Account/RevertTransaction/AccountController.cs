using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.AddTransaction;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Application.UseCases.AccountsUseCase.GetTransactions;
using Application.UseCases.AccountsUseCase.RevertTransaction;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.RevertTransaction
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRevertTransactionUseCase _useCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IRevertTransactionUseCase useCase)
        {
            _logger = logger;
            _configuration = configuration;
            _useCase = useCase;
        }

        [HttpPost("{accountId}/transactions/{transactionId}/revert")]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromRoute] string accountId, [FromRoute] string transactionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");
                
            var response = await _useCase.HandleAsync(accountId, transactionId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
