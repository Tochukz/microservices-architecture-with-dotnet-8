 # .NET Core Microservices - The Complete Guide (.NET 8 MVC)

## Resources
[Course Download](https://dotnetmastery.com/Home/Details?courseId=19)
[GitHub Code](https://github.com/bhrugen/Mango_Microservices)  

## Related resources
[Dotnet Mastery](https://dotnetmastery.com/Home/Vlog)  
[Dotnet Mastery Blog](https://dotnetmastery.com/Blog)  
[YouTube Channel](https://youtube.com/@DotNetMastery/videos)  
[Learn ASP.NET Core MVC (.NET 8) - The Complete Guide](https://www.youtube.com/watch?v=AopeJjkcRvU)  


## Section 1: Introduction
__Why Microservices__  
* Independently deployable
* Independently Scalable
* No bulky code
* Reduces downtime

__Tools Needed__  
* Visual Studio 2022 .NET 8
* SQL Server
* Azure Subscription

## Section 2:  Coupon API - Getting Started
__Solution Folder Structure__  
1. Create a Dotnet Solution using Visual Studio. You may need to create a project with a Solution and then delete the project.
2. Add the following _Solution folders_ in the Solution
  - Frontend
  - Gateway
  - Integration
  - Services

__Adding Swashbuckle to Project__
1. Create a sample Porject and Solution - `SwaggerSln`. We will setup Swagger on the `SwaggerApp`  project in the `SwaggerSln`
2. Add the Nuget package `Swashbuckle.AspNetCore` to the project.
3. Update `Program.cs` file.  
Remove the following lines of code shown by the commented lines
```cs
// builder.Services.AddOpenApi();
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
}
```

Add the following lines in their place
```cs
builder.Services.AddSwa ggerGen();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
4. Run the application on Visual Studio using IIS Express and then on the Browser, go to `https://localhost:44300/swagger/index.html`. You should find the _Swagger Documentation_.  
5. Update the `properties/launchSetting.json` so that the _Swagger Documentation_ is the default display page whenever the application is run.  
```json
{
  "profiles": {
    "http": {
       "commandName": "Project",
       "launchUrl": "swagger",
     },
    "https": {
      "commandName": "Project",
      "launchUrl": "swagger",
      ...
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchUrl": "swagger",
      ...
    },
    "iisSettings": {
      ...
      "iisExpress": {
        "applicationUrl": "http://localhost:60864/",
        "launchUrl": "swagger",
        ...
      }
  }
  }
}
```
Note that the options `  "launchUrl": "swagger"` has been added to all the _profiles_ i.e `http`, `http`, etc.


#### Coupon API
__AutoMapper Nuget Package__  
The CouponAPI requires the `AutoMapper` Nuget Package.   
Install the following Nuget Packages
- `AutoMapper`  
- `AutoMapper.Extensions.Microsoft.DependencyInjection` (deprecated)

__Entity Framework Core__  
Install the following Packages for Entity Framework Core:
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools` (for migrations)

__Authentication__  
Install the `JwtBearer` package
- `Microsoft.AspNetCore.Authentication.JwtBearer`

__Install Entity Framework CLI Tool__  
If you have not already done so, you may install the Entity Framework Dotnet CLI Tool
```bash
$ dotnet tool install --global dotnet-ef
$ dotnet ef --version
```

__Migration__  
After defining `Coupon` model, the `AppDbContext` and updating `Program.cs` to add the DbContext options to services, you can generate a migration to create the `Coupons`table
```bash
$ cd Mango.Services.CouponAPI
$ dotnet ef migrations add AddCouponToDb
```
You generate Migrations will be found in the `Migrations`folder.
Run the generated migration
```bash
$ dotnet ef database update
```  
Connect to you SQL Server Instance using SSMS and check to confirm if the `Coupons` table was created in the `Mango_Coupon` database.  
