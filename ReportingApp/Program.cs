using System;
using System.Globalization;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;
using DependencyResolver;
using Microsoft.Extensions.DependencyInjection;
using Northwind.ReportingServices.ProductReports;

namespace ReportingApp
{
    /// <summary>
    /// Program class.
    /// </summary>
    public sealed class Program
    {
        private const string CurrentProductsReport = "current-products";
        private const string MostExpensiveProductsReport = "most-expensive-products";
        private const string LessThanPriceProductsReport = "price-less-than-products";
        private const string MoreThanPriceProductsReport = "price-more-than-products";
        private const string BetweenPriceProductsReport = "price-between-products";
        private const string AboveAveragePriceProductsReport = "price-above-average-products";
        private const string BelowAveragePriceProductsReport = "price-below-average-products";
        private const string UnitsInStockDeficitProductsReport = "units-in-stock-deficit";
        private const string CurrentProductsLocalPricesReport = "current-products-local-prices";

        /// <summary>
        /// A program entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                ShowHelp();
                return;
            }

            var reportName = args[0];

            if (string.Equals(reportName, CurrentProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                await ShowCurrentProducts();
            }
            else if (string.Equals(reportName, AboveAveragePriceProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                await ShowAboveAveragePriceProducts();
            }
            else if (string.Equals(reportName, BelowAveragePriceProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                await ShowBelowAveragePriceProducts();
            }
            else if (string.Equals(reportName, UnitsInStockDeficitProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                await ShowUnitsInStockDeficitProducts();
            }
            else if (string.Equals(reportName, CurrentProductsLocalPricesReport, StringComparison.InvariantCultureIgnoreCase))
            {
                await ShowCurrentProductsLocalWithLocalPrices();
            }
            else if (string.Equals(reportName, MostExpensiveProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Length > 1 && int.TryParse(args[1], out int count))
                {
                    await ShowMostExpensiveProducts(count);
                }
            }
            else if (string.Equals(reportName, LessThanPriceProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Length > 1 && decimal.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
                {
                    await ShowLessThanPriceProducts(price);
                }
            }
            else if (string.Equals(reportName, MoreThanPriceProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Length > 1 && decimal.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
                {
                    await ShowMoreThanPriceProducts(price);
                }
            }
            else if (string.Equals(reportName, BetweenPriceProductsReport, StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Length > 2
                    && decimal.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal lowerPrice)
                    && decimal.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal upperPrice))
                {
                    await ShowBetweenPriceProducts(lowerPrice, upperPrice);
                }
            }
            else
            {
                ShowHelp();
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\tReportingApp.exe <report> <report-argument1> <report-argument2> ...");
            Console.WriteLine();
            Console.WriteLine("Reports:");
            Console.WriteLine($"\t{CurrentProductsReport}\t\tShows current products.");
            Console.WriteLine($"\t{MostExpensiveProductsReport}\t\tShows specified number of the most expensive products.");
        }

        private static async Task ShowCurrentProducts()
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetCurrentProducts();
            PrintProductReport("current products:", report);
        }

        private static async Task ShowCurrentProductsLocalWithLocalPrices()
        {
            var provider = new ResolverConfig().CreateServiceProvider();
            var service = provider.GetService<IProductReportService>();
            var exchangeService = provider.GetService<ICurrencyExchangeService>();
            var currencyService = provider.GetService<ICountryCurrencyService>();

            var report = await service.GetCurrentProductsWithLocalCurrencyReport(currencyService, exchangeService);
            await new CurrentProductLocalPriceReport(service, exchangeService, currencyService).PrintReport();
        }

        private static async Task ShowAboveAveragePriceProducts()
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetAboveAveragePriceProductsReport();
            PrintProductReport("products with price above average:", report);
        }

        private static async Task ShowBelowAveragePriceProducts()
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetBelowAveragePriceProductsReport();
            PrintProductReport("products with price below average:", report);
        }

        private static async Task ShowUnitsInStockDeficitProducts()
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetUnitsInStockDeficitProductsReport();
            PrintProductReport("products with units in stock deficit:", report);
        }

        private static async Task ShowMostExpensiveProducts(int count)
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetMostExpensiveProductsReport(count);
            PrintProductReport($"{count} most expensive products:", report);
        }

        private static async Task ShowLessThanPriceProducts(decimal price)
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetLessThanPriceProductsReport(price);
            PrintProductReport($"products with price less than {price.ToString(CultureInfo.InvariantCulture)}:", report);
        }

        private static async Task ShowMoreThanPriceProducts(decimal price)
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetMoreThanPriceProductsReport(price);
            PrintProductReport($"products with price more than {price.ToString(CultureInfo.InvariantCulture)}:", report);
        }

        private static async Task ShowBetweenPriceProducts(decimal lowerPrice, decimal upperPrice)
        {
            var service = new ResolverConfig().CreateServiceProvider().GetService<IProductReportService>();
            var report = await service.GetBetweenPriceProductsReport(lowerPrice, upperPrice);
            PrintProductReport($"products with price between {lowerPrice.ToString(CultureInfo.InvariantCulture)} and {upperPrice.ToString(CultureInfo.InvariantCulture)}:", report);
        }

        private static void PrintProductReport(string header, ProductReport<ProductPrice> productReport)
        {
            Console.WriteLine($"Report - {header}");
            foreach (var reportLine in productReport.Products)
            {
                Console.WriteLine("{0}, {1}", reportLine.Name, reportLine.Price.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
