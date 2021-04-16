![](https://www.cepres.com/wp-content/uploads/2020/02/logo-cepres-2020.svg)
# Introduction
This is a technical assessment created for Cepres based using **ASP.NET Core with Angular 11** startup template [ASP.NET Boilerplate](https://aspnetboilerplate.com/Pages/Documents).
 
User Interface is based on [AdminLTE theme](https://github.com/ColorlibHQ/AdminLTE).
 
# Screenshots

#### Sample Dashboard Page
![](_screenshots/module-zero-core-template-ui-home.png)

#### User Creation Modal
![](_screenshots/module-zero-core-template-ui-user-create-modal.png)

#### Login Page

![](_screenshots/module-zero-core-template-ui-login.png)



# Design
It is based on Domain-Driven-Design using the following technologies: 
1. Azure SQL database: has 1000 products ready for testing
2. [Azure Web API](https://gimc-api.azurewebsites.net/odata/product?$count=true&$orderby=Id%20desc&$skip=15&$top=15): with **OData V4** Protocol the standard & best way to create a fully CRUD RESTful API with all of query options supported ex: sorting, filtering, paging, grouping, multiple query options and advanced search.
3. C#, EF Core, .NET 5
4. [Azure Static App](https://kind-sea-0ee997803.azurestaticapps.net): typescript SPA with Syncfusion grid
# Technical Tasks
#|Task|Time
--|--|--
1|Create & setup Solutions back & fron end|2 h
2|Create entites & code first migration|1 h
3|Create seed stratgy to cover all test cases|2 h
5|Implement Client & Server-Side validations|2 h
5|Implement "Domain Service"|2 h
5|Implement Statistics Reporting "Application Service"|2 h 
3|Implement OData-API controllers "Distributed Service"|2 h
3|Create Authorization Permissions|2 h
3|Create Localization Keys|2 h
1|Implement Patients NG Component & routing|1
4|Implement Patient Statistics Modal Componet|2 h
4|Create, configure Syncfusion Patients grid|2 h
4|Create, configure Syncfusion Patient Visits grid|2 h
1|Properties Client-Side Formatting
6|Add Product Client-Side 20 validations|45 m
8|Create & push to GitHub repo|1 h
9|Test|2 h
10|Document|1 h
#|**TOTAL**|**9 h**
