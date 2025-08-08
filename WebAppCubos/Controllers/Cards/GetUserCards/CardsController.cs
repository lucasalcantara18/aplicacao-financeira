using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.GetCards;
using Application.UseCases.CardsUseCase.GetCards;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCubos.Controllers.Cards.GetUserCards
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ILogger<CardsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetUserCardsUseCase _getCardsUseCase;

        public CardsController(ILogger<CardsController> logger, IConfiguration configuration, IGetUserCardsUseCase getCardsUseCase)
        {
            _logger = logger;
            _configuration = configuration;
            _getCardsUseCase = getCardsUseCase;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetUserCards([FromQuery] CardsQueryParams qParams)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("UserId");

            var response = await _getCardsUseCase.HandleAsync(qParams.ItemsPerPage.Value, qParams.CurrentPage.Value, userId);

            if (response != null)
                return Ok(response);

            return UnprocessableEntity();
        }
    }
}
