using System.Collections.Generic;
using System.Text.Json.Serialization;

#pragma warning disable CA1812

namespace CurrencyServices.CurrencyExchange
{
    /// <summary>
    /// Represents exchange rates of currencies.
    /// </summary>
    internal class CurrencyExchangeRate
    {
        /// <summary>
        /// Gets or sets currency exchange rates.
        /// </summary>
        [JsonPropertyName("quotes")]
        public IDictionary<string, decimal> Rates { get; set; }
    }
}
