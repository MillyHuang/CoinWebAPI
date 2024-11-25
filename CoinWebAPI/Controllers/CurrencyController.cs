using CoinWebAPI.Entities;
using CoinWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ApplicationDbContext context, ILogger<CurrencyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Query
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
        {
            _logger.LogInformation("Fetching all currencies from the database.");
            try
            {
                var currencies = await _context.Currencies.OrderBy(c => c.Code).ToListAsync();
                _logger.LogInformation("Successfully fetched {Count} currencies.", currencies.Count);
                return currencies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching currencies.");
                return StatusCode(500, "Internal server error");
            }
        }

        //New
        [HttpPost]
        public async Task<ActionResult<Currency>> AddCurrency(Currency currency)
        {
            _logger.LogInformation("Adding a new currency: {Code}, {Name}", currency.Code, currency.Name);
            try
            {
                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added currency with ID: {Id}", currency.Id);
                return CreatedAtAction(nameof(GetCurrencies), new { id = currency.Id }, currency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new currency.");
                return StatusCode(500, "Internal server error");
            }
        }

        //Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurrency(int id, Currency currency)
        {
            if (id != currency.Id)
            {
                _logger.LogWarning("Update failed: Mismatch between route ID ({RouteId}) and currency ID ({CurrencyId}).", id, currency.Id);
                return BadRequest("ID mismatch");
            }

            try
            {
                _context.Entry(currency).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated currency with ID: {Id}", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency issue occurred while updating currency with ID: {Id}", id);
                return StatusCode(500, "Concurrency error occurred");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating currency with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            try
            {
                var currency = await _context.Currencies.FindAsync(id);
                if (currency == null)
                {
                    _logger.LogWarning("Delete failed: Currency with ID {Id} not found.", id);
                    return NotFound();
                }

                _context.Currencies.Remove(currency);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted currency with ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting currency with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
