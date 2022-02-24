using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;

namespace CurrencyServices.CurrencyExchange
{
    /// <inheritdoc cref="ICurrencyExchangeService"/>
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly string accessKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyExchangeService"/> class.
        /// </summary>
        /// <param name="accessKey">Access key.</param>
        public CurrencyExchangeService(string accessKey)
        {
            this.accessKey = !string.IsNullOrWhiteSpace(accessKey) ? accessKey : throw new ArgumentException("Access key is invalid.", nameof(accessKey));
        }

        /// <inheritdoc/>
        public async Task<decimal> GetCurrencyExchangeRate(string baseCurrency, string exchangeCurrency)
        {
            CurrencyExchangeRate exchangeRates;

            using (HttpClient client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(new Uri($"http://api.currencylayer.com/live?access_key={this.accessKey}"));
                exchangeRates = await JsonSerializer.DeserializeAsync<CurrencyExchangeRate>(stream);
            }

            if (exchangeRates.Rates.TryGetValue($"USD{exchangeCurrency}", out decimal exchangeCurrencyRate)
                && exchangeRates.Rates.TryGetValue($"USD{baseCurrency}", out decimal baseCurrencyRate))
            {
                return exchangeCurrencyRate / baseCurrencyRate;
            }
            else
            {
                throw new ArgumentException("Incorrect currencies.");
            }
        }
    }
}
