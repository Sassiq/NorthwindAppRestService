using System.Threading.Tasks;

namespace CurrencyServices.Interfaces
{
    /// <summary>
    /// Service for operating with currency exchange rates.
    /// </summary>
    public interface ICurrencyExchangeService
    {
        /// <summary>
        /// Gets currency exchange rate.
        /// </summary>
        /// <param name="baseCurrency">Base currency.</param>
        /// <param name="exchangeCurrency">Exchange currency.</param>
        /// <returns>Task which represents currency exchange rate.</returns>
        Task<decimal> GetCurrencyExchangeRate(string baseCurrency, string exchangeCurrency);
    }
}
