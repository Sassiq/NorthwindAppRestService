using System;
using System.IO;
using CurrencyServices.CountryCurrency;
using CurrencyServices.CurrencyExchange;
using CurrencyServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.ReportingServices.OData.ProductReports;
using Northwind.ReportingServices.ProductReports;

namespace DependencyResolver
{
    public class ResolverConfig
    {
        public IConfiguration ConfigurationRoot { get; } =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        public IServiceProvider CreateServiceProvider()
        {
            var a = this.ConfigurationRoot.GetConnectionString("SqlConnection");
            return this.ConfigurationRoot["type"] switch
            {
                "REST" => new ServiceCollection()
                    .AddScoped<ICurrencyExchangeService>(s => new CurrencyExchangeService(this.ConfigurationRoot["accessKey"]))
                    .AddScoped<ICountryCurrencyService, CountryCurrencyService>()
                    .AddScoped<IProductReportService>(s => new ProductReportService(new Uri(this.ConfigurationRoot["northwindUrl"])))
                    .BuildServiceProvider(),

                "SQL" => new ServiceCollection()
                    .AddScoped<ICurrencyExchangeService>(s => new CurrencyExchangeService(this.ConfigurationRoot["accessKey"]))
                    .AddScoped<ICountryCurrencyService, CountryCurrencyService>()
                    .AddScoped<IProductReportService>(s => new Northwind.ReportingServices.SqlService.ProductReports.ProductReportService(this.ConfigurationRoot.GetConnectionString("SqlConnection")))
                    .BuildServiceProvider(),

                _ => throw new ArgumentException("Incorrect appsettings.json"),
            };
        }
    }
}
