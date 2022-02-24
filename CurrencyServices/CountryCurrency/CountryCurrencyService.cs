using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;

#pragma warning disable CA1822 //Class doesn't contain instance fields and can be marked as static.

namespace CurrencyServices.CountryCurrency
{
    /// <inheritdoc cref="ICountryCurrencyService"/>
    public class CountryCurrencyService : ICountryCurrencyService
    {
        /// <inheritdoc/>
        public async Task<LocalCurrency> GetLocalCurrencyByCountry(string countryName)
        {
            try
            {
                LocalCurrency result;

                using (HttpClient client = new HttpClient())
                {
                    var task = client.GetStreamAsync(new Uri($"https://restcountries.com/v2/name/{countryName}?fields=name,numericCode,currencies"));
                    var documentRoot = (await JsonDocument.ParseAsync(await task)).RootElement[0];

                    result = new LocalCurrency()
                    {
                        CountryName = documentRoot.GetProperty("name").GetString(),
                        CurrencyCode = documentRoot.GetProperty("currencies")[0].GetProperty("code").GetString(),
                        CurrencySymbol = documentRoot.GetProperty("currencies")[0].GetProperty("symbol").GetString(),
                    };
                }

                return result;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Can't find country {countryName}.");
            }
        }
    }
}
