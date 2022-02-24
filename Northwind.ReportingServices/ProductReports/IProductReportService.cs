using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;

namespace Northwind.ReportingServices.ProductReports
{
    /// <summary>
    /// Represents a service that produces product-related reports.
    /// </summary>
    public interface IProductReportService
    {
        /// <summary>
        /// Gets a product report with all current products.
        /// </summary>
        /// <returns>Returns <see cref="ProductReport{T}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetCurrentProducts();

        /// <summary>
        /// Gets a product report with most expensive products.
        /// </summary>
        /// <param name="count">Items count.</param>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetMostExpensiveProductsReport(int count);

        /// <summary>
        /// Gets a product report with the price less than the given.
        /// </summary>
        /// <param name="price">Given price.</param>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetLessThanPriceProductsReport(decimal price);

        /// <summary>
        /// Gets a product report with the price more than the given.
        /// </summary>
        /// <param name="price">Given price.</param>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetMoreThanPriceProductsReport(decimal price);

        /// <summary>
        /// Gets a product report with the price less than the upperPrice abd more than lowerPrice.
        /// </summary>
        /// <param name="lowerPrice">Lower price.</param>
        /// <param name="upperPrice">Upper price.</param>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetBetweenPriceProductsReport(decimal lowerPrice, decimal upperPrice);

        /// <summary>
        /// Gets a product report with the price above average.
        /// </summary>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetAboveAveragePriceProductsReport();

        /// <summary>
        /// Gets a product report with the price below average.
        /// </summary>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetBelowAveragePriceProductsReport();

        /// <summary>
        /// Gets a product report with units in stock deficit.
        /// </summary>
        /// <returns>Returns <see cref="ProductReport{ProductPrice}"/>.</returns>
        Task<ProductReport<ProductPrice>> GetUnitsInStockDeficitProductsReport();

        /// <summary>
        /// Gets a product report with local currencies.
        /// </summary>
        /// <param name="countryCurrencyService">Country currency service.</param>
        /// <param name="currencyExchangeService">Currency exchange service.</param>
        /// <returns>Returns <see cref="ProductLocalPrice"/>.</returns>
        Task<ProductReport<ProductLocalPrice>> GetCurrentProductsWithLocalCurrencyReport(ICountryCurrencyService countryCurrencyService, ICurrencyExchangeService currencyExchangeService);
    }
}
