using System.Threading.Tasks;

namespace CurrencyServices.Interfaces
{
    /// <summary>
    /// Service for operating with <see cref="LocalCurrency"/>.
    /// </summary>
    public interface ICountryCurrencyService
    {
        /// <summary>
        /// Gets <see cref="LocalCurrency"/>.
        /// </summary>
        /// <param name="countryName">Country name.</param>
        /// <returns>Task which represents <see cref="LocalCurrency"/>.</returns>
        Task<LocalCurrency> GetLocalCurrencyByCountry(string countryName);
    }
}
