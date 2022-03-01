using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CurrencyServices.Interfaces;
using Northwind.ReportingServices.ProductReports;

namespace Northwind.ReportingServices.SqlService.ProductReports
{
    public class ProductReportService : IProductReportService
    {
        private const string StandardCurrency = "USD";
        private readonly SqlConnection connection;

        public ProductReportService(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public async Task<ProductReport<ProductPrice>> GetAboveAveragePriceProductsReport()
        {
            using (var sqlCommand =
                   new SqlCommand("GetAboveAveragePriceProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetBelowAveragePriceProductsReport()
        {
            using (var sqlCommand =
                   new SqlCommand("GetBelowAveragePriceProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetBetweenPriceProductsReport(decimal lowerPrice, decimal upperPrice)
        {
            using (var sqlCommand =
                   new SqlCommand("GetBetweenPriceProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                sqlCommand.Parameters.Add("@lowerPrice", SqlDbType.Decimal);
                sqlCommand.Parameters["@lowerPrice"].Value = lowerPrice;

                sqlCommand.Parameters.Add("@upperPrice", SqlDbType.Decimal);
                sqlCommand.Parameters["@upperPrice"].Value = upperPrice;

                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetCurrentProducts()
        {
            using (var sqlCommand =
                   new SqlCommand("GetCurrentProducts", this.connection) {CommandType = CommandType.StoredProcedure})
            {
                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductLocalPrice>> GetCurrentProductsWithLocalCurrencyReport(ICountryCurrencyService countryCurrencyService, ICurrencyExchangeService currencyExchangeService)
        {
            using (var sqlCommand =
                   new SqlCommand("GetCurrentLocalProducts", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductLocalPrice>();

                while (await reader.ReadAsync())
                {
                    var report = await GetProductLocalPrice(reader, countryCurrencyService, currencyExchangeService);

                    reports.Add(report);
                }

                return new ProductReport<ProductLocalPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetLessThanPriceProductsReport(decimal price)
        {
            using (var sqlCommand =
                   new SqlCommand("GetLessThanPriceProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                sqlCommand.Parameters.Add("@price", SqlDbType.Decimal);
                sqlCommand.Parameters["@price"].Value = price;

                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetMoreThanPriceProductsReport(decimal price)
        {
            using (var sqlCommand =
                   new SqlCommand("GetMoreThanPriceProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                sqlCommand.Parameters.Add("@price", SqlDbType.Decimal);
                sqlCommand.Parameters["@price"].Value = price;

                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetMostExpensiveProductsReport(int count)
        {
            using (var sqlCommand =
                   new SqlCommand("GetMostExpensiveProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                sqlCommand.Parameters.Add("@count", SqlDbType.Decimal);
                sqlCommand.Parameters["@count"].Value = count;

                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        public async Task<ProductReport<ProductPrice>> GetUnitsInStockDeficitProductsReport()
        {
            using (var sqlCommand =
                   new SqlCommand("GetUnitsInStockDeficitProductsReport", this.connection) { CommandType = CommandType.StoredProcedure })
            {
                await this.connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();

                var reports = new List<ProductPrice>();

                while (await reader.ReadAsync())
                {
                    var report = GetProductPrice(reader);

                    reports.Add(report);
                }

                return new ProductReport<ProductPrice>(reports);
            }
        }

        private ProductPrice GetProductPrice(SqlDataReader reader)
        {
            var name = (string)reader["ProductName"];
            var price = (decimal)reader["UnitPrice"];

            return new ProductPrice() {Name = name, Price = price};
        }

        private async Task<ProductLocalPrice> GetProductLocalPrice(SqlDataReader reader, ICountryCurrencyService countryCurrencyService, ICurrencyExchangeService currencyExchangeService)
        {
            var name = (string)reader["ProductName"];
            var price = (decimal)reader["UnitPrice"];
            var country = (string)reader["Country"];

            var report = new ProductLocalPrice() {Name = name, Price = price, Country = country};

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

            return report;
        }
    }
}
