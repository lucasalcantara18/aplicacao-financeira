using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.AddTransaction;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Application.UseCases.AccountsUseCase.GetTransactions;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Account.GetTransactions
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetTransactionsUseCase _useCase;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IGetTransactionsUseCase useCase)
        {
            _logger = logger;
            _configuration = configuration;
            _useCase = useCase;
        }

        [HttpGet("{accountId}/transactions")]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromRoute] string accountId, [FromQuery] TransactionsQueryParams qParams)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");
                
            var response = await _useCase.HandleAsync(qParams.ItemsPerPage.Value, qParams.CurrentPage.Value, qParams.Type, accountId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
