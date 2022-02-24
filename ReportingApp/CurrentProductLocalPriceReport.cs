using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;
using Northwind.ReportingServices.ProductReports;

namespace ReportingApp
{
    /// <summary>
    /// Prints current product with local price.
    /// </summary>
    public class CurrentProductLocalPriceReport
    {
        private readonly IProductReportService productReportService;
        private readonly ICurrencyExchangeService currencyExchangeService;
        private readonly ICountryCurrencyService countryCurrencyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentProductLocalPriceReport"/> class.
        /// </summary>
        /// <param name="productReportService">Product report service.</param>
        /// <param name="currencyExchangeService">Currency exchange service.</param>
        /// <param name="countryCurrencyService">Country currency service.</param>
        public CurrentProductLocalPriceReport(IProductReportService productReportService, ICurrencyExchangeService currencyExchangeService, ICountryCurrencyService countryCurrencyService)
        {
            this.productReportService = productReportService ?? throw new ArgumentNullException(nameof(productReportService));
            this.currencyExchangeService = currencyExchangeService ?? throw new ArgumentNullException(nameof(currencyExchangeService));
            this.countryCurrencyService = countryCurrencyService ?? throw new ArgumentNullException(nameof(countryCurrencyService));
        }

        /// <summary>
        /// Prints current local price reports in Console.
        /// </summary>
        /// <returns>Nothing.</returns>
        public async Task PrintReport()
        {
            var reports = await this.productReportService.GetCurrentProductsWithLocalCurrencyReport(this.countryCurrencyService, this.currencyExchangeService);

            Console.WriteLine($"Report - current products with local price:");
            foreach (var reportLine in reports.Products)
            {
                Console.WriteLine("{0}, {1}$, {2}, {3}{4}", reportLine.Name, reportLine.Price.ToString(CultureInfo.InvariantCulture), reportLine.Country, reportLine.LocalPrice.ToString(CultureInfo.InvariantCulture), reportLine.CurrencySymbol);
            }
        }
    }
}
