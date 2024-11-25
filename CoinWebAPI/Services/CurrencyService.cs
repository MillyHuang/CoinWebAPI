using CoinWebAPI.Entities;
using CoinWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinWebAPI.Services
{
    public class CurrencyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CoindeskService> _logger;

        public CurrencyService(ApplicationDbContext context, ILogger<CoindeskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 查詢所有幣別，依幣別代碼排序
        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all currencies from the database.");
                var currencies = await _context.Currencies
                    .OrderBy(c => c.Code) // 以 Code 排序
                    .ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} currencies.", currencies.Count);
                return currencies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching currencies.");
                throw;
            }
        }

        // 根據 ID 查詢特定幣別
        public async Task<Currency> GetCurrencyByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching currency with ID: {Id}.", id);
                var currency = await _context.Currencies.FindAsync(id);
                if (currency == null)
                {
                    _logger.LogWarning("Currency with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Currency with ID {id} not found.");
                }
                return currency;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching currency with ID: {Id}.", id);
                throw;
            }
        }

        // 新增幣別
        public async Task AddCurrencyAsync(Currency currency)
        {
            try
            {
                _logger.LogInformation("Adding new currency with Code: {Code}.", currency.Code);
                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added currency with Code: {Code}.", currency.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new currency.");
                throw;
            }
        }

        // 修改幣別
        public async Task UpdateCurrencyAsync(Currency currency)
        {
            try
            {
                _logger.LogInformation("Updating currency with ID: {Id}.", currency.Id);
                var existingCurrency = await _context.Currencies.FindAsync(currency.Id);
                if (existingCurrency != null)
                {
                    existingCurrency.Code = currency.Code;
                    existingCurrency.Name = currency.Name;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully updated currency with ID: {Id}.", currency.Id);
                }
                else
                {
                    _logger.LogWarning("Currency with ID: {Id} not found for update.", currency.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating currency with ID: {Id}.", currency.Id);
                throw;
            }
        }

        // 刪除幣別
        public async Task DeleteCurrencyAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting currency with ID: {Id}.", id);
                var currency = await _context.Currencies.FindAsync(id);
                if (currency != null)
                {
                    _context.Currencies.Remove(currency);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully deleted currency with ID: {Id}.", id);
                }
                else
                {
                    _logger.LogWarning("Currency with ID: {Id} not found for deletion.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting currency with ID: {Id}.", id);
                throw;
            }
        }
    }
}

