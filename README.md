# Northwind Application

Console application, which gets custom product reports from Northwind database using OData or ADO.NET.  
User can switch the way with "type" parameter in [appsettings.json](https://github.com/Sassiq/NorthwindConsoleApplication/blob/main/appsettings.json).

Report includes:
* Product name.
* Product price in dollars.
* Fullname of the producing country (from external service)
* Product price in local currency of the producing country.
* Currency of the producing country (most symbols will be shown as "?").

Example:  
```sh
$ .\ReportingApp.exe current-products-local-prices
Report - current products with local price:
Aniseed Syrup, 10$, United Kingdom of Great Britain and Northern Ireland, 08?
Boston Crab Meat, 18$, United States of America, 18$
Camembert Pierrot, 34$, France, 31?
Carnarvon Tigers, 63$, Australia, 93$
Chai, 18$, United Kingdom of Great Britain and Northern Ireland, 14?
...
```

Available commands:
- current-products
- most-expensive-products [arg1]
- price-less-than-products [arg1]
- price-more-than-products [arg1]
- price-between-products [arg1] [arg2]
- price-above-average-products
- price-below-average-products
- units-in-stock-deficit
- current-products-local-prices
