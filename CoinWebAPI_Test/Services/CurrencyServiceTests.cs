using CoinWebAPI.Entities;
using CoinWebAPI.Models;
using CoinWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


namespace CoinWebAPI_Test.Services
{
    public class CurrencyServiceTests
    {

        //測試新增
        [Fact]
        public async Task AddCurrency_ShouldAddToDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mockLogger = new Mock<ILogger<CoindeskService>>();
            var service = new CurrencyService(dbContext, mockLogger.Object);
            var newCurrency = new Currency { Code = "USD", Name = "美金" };

            // Act
            await service.AddCurrencyAsync(newCurrency);
            var result = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "USD");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.Code);
            Assert.Equal("美金", result.Name);
        }
        //測試查詢
        [Fact]
        public async Task GetCurrency_ShouldReturnCorrectCurrency()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Currencies.Add(new Currency { Code = "USD", Name = "美金" });
            dbContext.Currencies.Add(new Currency { Code = "EUR", Name = "歐元" });
            await dbContext.SaveChangesAsync();
            var mockLogger = new Mock<ILogger<CoindeskService>>();
            var service = new CurrencyService(dbContext, mockLogger.Object);

            // Act
            var result = await service.GetCurrenciesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("EUR", result[0].Code); //order by code,EUR在前,USD在後
            Assert.Equal("USD", result[1].Code);
        }
        //測試修改
        [Fact]
        public async Task UpdateCurrency_ShouldModifyExistingCurrency()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Currencies.Add(new Currency { Id = 1, Code = "USD", Name = "美金" });
            await dbContext.SaveChangesAsync();
            var mockLogger = new Mock<ILogger<CoindeskService>>();
            var service = new CurrencyService(dbContext, mockLogger.Object);
            var updatedCurrency = new Currency { Id = 1, Code = "USD", Name = "美元" };

            // Act
            await service.UpdateCurrencyAsync(updatedCurrency);
            var result = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.Code);
            Assert.Equal("美元", result.Name);
        }
        //測試刪除
        [Fact]
        public async Task DeleteCurrency_ShouldRemoveFromDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Currencies.Add(new Currency { Id = 1, Code = "USD", Name = "美金" });
            await dbContext.SaveChangesAsync();
            var mockLogger = new Mock<ILogger<CoindeskService>>();
            var service = new CurrencyService(dbContext, mockLogger.Object);

            // Act
            await service.DeleteCurrencyAsync(1);
            var result = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Id == 1);

            // Assert
            Assert.Null(result);
        }

        // 建立模擬的 In-Memory Database
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
