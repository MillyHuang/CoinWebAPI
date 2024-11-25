using CoinWebAPI.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace CoinWebAPI_Test.Services
{
    public class CoindeskServiceTests
    {
        [Fact]
        public async Task GetExchangeRates_ShouldReturnTransformedData()
        {
            // Arrange
            var mockHttpClient = new Mock<IHttpClientFactory>();
            var mockResponse = new HttpResponseMessage
            {
                Content = new StringContent("{\"bpi\":{\"USD\":{\"rate_float\":1.0}}}")
            };

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            //var httpClient = new HttpClient(mockHandler.Object);
            //mockHttpClient.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            //var service = new CoindeskService(httpClient);

            // Mock IHttpClientFactory
            var httpClient = new HttpClient(mockHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            // Mock ILogger
            var mockLogger = new Mock<ILogger<CoindeskService>>();
            // Mock ILocalizer
            var mockLocalizer = new Mock<IStringLocalizer<CoindeskService>>();
            var service = new CoindeskService(mockHttpClientFactory.Object, mockLogger.Object, mockLocalizer.Object);

            // Act
            var result = await service.GetExchangeRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.Rates, r => r.Code == "USD");
        }


    }
}
