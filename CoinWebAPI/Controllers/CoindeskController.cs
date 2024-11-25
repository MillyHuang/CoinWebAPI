using Microsoft.AspNetCore.Mvc;
using CoinWebAPI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace CoinWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoindeskController : ControllerBase
    {
        private readonly CoindeskService _service;
        private readonly ILogger<CoindeskController> _logger;
        private readonly IStringLocalizer<CoindeskController> _localizer;
        public CoindeskController(CoindeskService service, ILogger<CoindeskController> logger, IStringLocalizer<CoindeskController> localizer)
        {
            _service = service;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        /// Fetch and transform data from Coindesk API.
        /// </summary>
        /// <returns>A transformed response with exchange rates and metadata.</returns>
        [HttpGet("coindesk")]
        public async Task<IActionResult> GetCoindeskData()
        {
            _logger.LogInformation("API 'GetCoindeskData' called at {Time}", DateTime.UtcNow);

            try
            {
                var result = await _service.GetExchangeRatesAsync();
                _logger.LogInformation("API 'GetCoindeskData' response: {Response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in 'GetCoindeskData' at {Time}", DateTime.UtcNow);
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }


    }
}

