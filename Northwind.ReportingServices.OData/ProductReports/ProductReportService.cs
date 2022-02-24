using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;
using Northwind.ReportingServices.ProductReports;
using NorthwindModel;
using NorthwindProduct = NorthwindModel.Product;

#pragma warning disable CA2008

namespace Northwind.ReportingServices.OData.ProductReports
{
    /// <inheritdoc cref="IProductReportService"/>
    public class ProductReportService : IProductReportService
    {
        private const string StandardCurrency = "USD";
        private readonly NorthwindModel.NorthwindEntities entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductReportService"/> class.
        /// </summary>
        /// <param name="northwindServiceUri">An URL to Northwind OData service.</param>
        public ProductReportService(Uri northwindServiceUri)
        {
            this.entities = new NorthwindModel.NorthwindEntities(northwindServiceUri ?? throw new ArgumentNullException(nameof(northwindServiceUri)));
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetCurrentProducts()
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where !p.Discontinued
                orderby p.ProductName
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice ?? 0,
                });

            var result = await Task<IEnumerable<ProductPrice>>
                .Factory
                .FromAsync(query.BeginExecute(null, query), endResult => query.EndExecute(endResult))
                .ContinueWith(t => ContinuePage(t.Result));

            return new ProductReport<ProductPrice>(result);

            IEnumerable<ProductPrice> ContinuePage(IEnumerable<ProductPrice> response)
            {
                foreach (var element in response)
                {
                    yield return element;
                }

                if ((response as QueryOperationResponse)?.GetContinuation() is DataServiceQueryContinuation<ProductPrice> continuation)
                {
                    var innerTask = Task<IEnumerable<ProductPrice>>
                        .Factory
                        .FromAsync(this.entities.BeginExecute(continuation, null, null), this.entities.EndExecute<ProductPrice>)
                        .ContinueWith(t => ContinuePage(t.Result));

                    foreach (var productPrice in innerTask.Result)
                    {
                        yield return productPrice;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetMostExpensiveProductsReport(int count)
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where p.UnitPrice != null
                orderby p.UnitPrice.Value descending
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice.Value,
                })
                .Take(count);

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            return new ProductReport<ProductPrice>(result);
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetLessThanPriceProductsReport(decimal price)
        {
            var query = (DataServiceQuery<ProductPrice>)(
                    from p in this.entities.Products
                    where p.UnitPrice != null && p.UnitPrice.Value < price
                    orderby p.ProductName
                    select new ProductPrice
                    {
                        Name = p.ProductName,
                        Price = p.UnitPrice.Value,
                    });

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            return new ProductReport<ProductPrice>(result);
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetMoreThanPriceProductsReport(decimal price)
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where p.UnitPrice != null && p.UnitPrice.Value > price
                orderby p.ProductName
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice.Value,
                });

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            return new ProductReport<ProductPrice>(result);
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetBetweenPriceProductsReport(decimal lowerPrice, decimal upperPrice)
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where p.UnitPrice != null && p.UnitPrice.Value < upperPrice && p.UnitPrice.Value > lowerPrice
                orderby p.ProductName
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice.Value,
                });

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            return new ProductReport<ProductPrice>(result);
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetAboveAveragePriceProductsReport()
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where p.UnitPrice != null
                orderby p.ProductName
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice.Value,
                });

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            var resultList = result.ToList();

            return new ProductReport<ProductPrice>(resultList.Where(p => p.Price > resultList.Average(n => n.Price)));
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetBelowAveragePriceProductsReport()
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where p.UnitPrice != null
                orderby p.ProductName
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice.Value,
                });

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            var resultList = result.ToList();

            return new ProductReport<ProductPrice>(resultList.Where(p => p.Price < resultList.Average(n => n.Price)));
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductPrice>> GetUnitsInStockDeficitProductsReport()
        {
            var query = (DataServiceQuery<ProductPrice>)(
                from p in this.entities.Products
                where p.UnitsInStock != null && p.UnitsOnOrder != null && p.UnitsInStock < p.UnitsOnOrder
                orderby p.ProductName
                select new ProductPrice
                {
                    Name = p.ProductName,
                    Price = p.UnitPrice.Value,
                });

            var result = await Task<IEnumerable<ProductPrice>>.Factory.FromAsync(query.BeginExecute(null, null), (ar) =>
            {
                return query.EndExecute(ar);
            });

            return new ProductReport<ProductPrice>(result);
        }

        /// <inheritdoc/>
        public async Task<ProductReport<ProductLocalPrice>> GetCurrentProductsWithLocalCurrencyReport(ICountryCurrencyService countryCurrencyService, ICurrencyExchangeService currencyExchangeService)
        {
            if (countryCurrencyService == null)
            {
                throw new ArgumentNullException(nameof(countryCurrencyService));
            }

            if (currencyExchangeService == null)
            {
                throw new ArgumentNullException(nameof(currencyExchangeService));
            }

            var productsQuery = (DataServiceQuery<NorthwindProduct>)(
                from p in this.entities.Products
                where !p.Discontinued
                select p);

            var suppliersQuery = (DataServiceQuery<Supplier>)this.entities.Suppliers;

            var products = await Task<IEnumerable<NorthwindProduct>>.Factory.FromAsync(productsQuery.BeginExecute(null, null), (ar) => productsQuery.EndExecute(ar));

            var suppliers = await Task<IEnumerable<Supplier>>.Factory.FromAsync(suppliersQuery.BeginExecute(null, null), (ar) => suppliersQuery.EndExecute(ar));

            var result = await GetProductLocalPrices(countryCurrencyService, currencyExchangeService, products, suppliers);

            return new ProductReport<ProductLocalPrice>(result);
        }

        private static async Task<IEnumerable<ProductLocalPrice>> GetProductLocalPrices(
            ICountryCurrencyService countryCurrencyService,
            ICurrencyExchangeService currencyExchangeService,
            IEnumerable<NorthwindProduct> products,
            IEnumerable<Supplier> suppliers)
        {
            var result = new List<ProductLocalPrice>(
                from product in products
                join supplier in suppliers on product.SupplierID equals supplier.SupplierID
                select new ProductLocalPrice()
                {
                    Country = supplier.Country,
                    Price = product.UnitPrice ?? 0,
                    Name = product.ProductName,
                });

            foreach (var report in result)
            {
                var localCurrency = await countryCurrencyService.GetLocalCurrencyByCountry(report.Country);
                report.CurrencySymbol = localCurrency.CurrencySymbol;
                report.Country = localCurrency.CountryName;

                if (localCurrency.CurrencyCode != StandardCurrency)
                {
                    report.LocalPrice =
                        (await currencyExchangeService.GetCurrencyExchangeRate(
                            StandardCurrency,
                            localCurrency.CurrencyCode)) * report.Price;
                }
                else
                {
                    report.LocalPrice = report.Price;
                }
            }

            return result;
        }
    }
}
