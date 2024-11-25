
namespace CoinWebAPI.Services
{
    public class ExchangeRateResponse
    {
        public string UpdatedAt { get; internal set; }
        public List<ExchangeRate> Rates { get; set; }
    }
}