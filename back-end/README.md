## Overview

This project provides the backend API for managing study information system using ASP.NET Core. 

---

## Patterns and technologies used

- Model View Controller – MVC 
- ASP.NET Core
- SOLID principles, Clean Architecture 
- Entity Framework (ORM)
- Unit of Work / Repository / Dependency Injection / Factory 
- Rest / JSON 
- JWT Authentication
- Docker

## Useful commands in .net console CLI

Install tooling

~~~bash
dotnet tool update -g dotnet-ef
dotnet tool update -g dotnet-aspnet-codegenerator 
~~~

## EF Core migrations

Run from solution folder  

~~~bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add FOOBAR
dotnet ef database   --project App.DAL.EF --startup-project WebApp update
dotnet ef database   --project App.DAL.EF --startup-project WebApp drop
~~~


## MVC controllers

Install from nuget:  
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer


Run from WebApp folder!  

~~~bash
cd WebApp

dotnet aspnet-codegenerator controller -name SubjectsController        -actions -m  App.Domain.Subject       -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
# use area
dotnet aspnet-codegenerator controller -name RefreshTokensController        -actions -m  App.Domain.Identity.AppRefreshToken        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -name SubjectsController        -actions -m  App.Domain.Subject        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

cd ..
~~~

Api controllers
~~~bash
dotnet aspnet-codegenerator controller -name SubjectsController  -m  App.Domain.Subjects        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~