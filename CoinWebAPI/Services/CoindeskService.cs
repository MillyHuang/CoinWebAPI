
using System.Net.Http;
using System.Text.Json;
using CoinWebAPI.Controllers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CoinWebAPI.Services
{
    public class CoindeskService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CoindeskService> _logger;
        private readonly IStringLocalizer<CoindeskService> _localizer;
        public CoindeskService(IHttpClientFactory httpClientFactory, ILogger<CoindeskService> logger, IStringLocalizer<CoindeskService> localizer)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<ExchangeRateResponse> GetExchangeRatesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var url = "https://api.coindesk.com/v1/bpi/currentprice.json";
                _logger.LogInformation("Sending request to external API: {Url} at {Time}", url, DateTime.UtcNow);

                var response = await httpClient.GetStringAsync(url);
                _logger.LogInformation("Received response from external API at {Time}: {Response}", DateTime.UtcNow, response);

                var json = JsonDocument.Parse(response);

                var rates = json.RootElement.GetProperty("bpi")
                    .EnumerateObject()
                    .Select(bpi => new ExchangeRate
                    {
                        Code = bpi.Name,
                        Rate = bpi.Value.GetProperty("rate_float").GetDouble()
                    })
                    .ToList();

                return new ExchangeRateResponse
                {
                    UpdatedAt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    Rates = rates
                };
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling external API at {Time}", DateTime.UtcNow);
                throw;
            }
}
    }
     //用於解析 Coindesk API 的 JSON 格式
        public class CoindeskResponse
    {
        public TimeInfo Time { get; set; }
        public Dictionary<string, CurrencyInfo> Bpi { get; set; }
    }

    public class TimeInfo
    {
        public string UpdatedISO { get; set; }
    }

    public class CurrencyInfo
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public float RateFloat { get; set; }
    }
    //public class ExchangeRateResponse
    //{
    //    public string UpdatedAt { get; set; }
    //    public List<ExchangeRate> Rates { get; set; }
    //}

    //public class ExchangeRate
    //{
    //    public string Code { get; set; }
    //    public double Rate { get; set; }
    //}
}
