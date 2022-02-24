using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.ReportingServices.ProductReports
{
    /// <summary>
    /// Represents a local product report.
    /// </summary>
    public class ProductLocalPrice
    {
        /// <summary>
        /// Gets or sets name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets local price.
        /// </summary>
        public decimal LocalPrice { get; set; }

        /// <summary>
        /// Gets or sets currency symbol.
        /// </summary>
        public string CurrencySymbol { get; set; }
    }
}
