﻿namespace CurrencyServices.Interfaces
{
    /// <summary>
    /// Represents local currency.
    /// </summary>
    public class LocalCurrency
    {
        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets currency code.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets currency symbol.
        /// </summary>
        public string CurrencySymbol { get; set; }
    }
}
