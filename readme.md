 # .NET Core Microservices - The Complete Guide (.NET 8 MVC)

## Resources
[Course Download](https://dotnetmastery.com/Home/Details?courseId=19)
[GitHub Code](https://github.com/bhrugen/Mango_Microservices)  

## Related resources
[Dotnet Mastery](https://dotnetmastery.com/Home/Vlog)  
[Dotnet Mastery Blog](https://dotnetmastery.com/Blog)  
[YouTube Channel](https://youtube.com/@DotNetMastery/videos)  
[Learn ASP.NET Core MVC (.NET 8) - The Complete Guide](https://www.youtube.com/watch?v=AopeJjkcRvU)  

### Running the example code
To run the [GitHub Example Code](https://github.com/bhrugen/Mango_Microservices) you may need to install the target dotnet framework version on your machine if you don't already have it.  
1. Check the Target Framework Version
- Right click on any of the Project > _Edit Project File_
- Check the `TargetFramework` version in the `csroj` file.
2. Check if you have the Dotnet Framework version installed
```bash
$ dotnet --list-sdks
```
3. Download and install the Dotnet Framework version if needed.  
- Go to [Download .NET](https://dotnet.microsoft.com/en-us/download/dotnet) and download _.Net 8.0_
- Run the downloaded executable to install the SDK version.

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
# Generate Mimration from Models
$ dotnet ef migrations add AddCouponToDb
```
Your generated migration file will be found in the `Migrations` folder, such as `Migrations/20251230021735_AddCouponToDb.cs`.    
Run the generated migration
```bash
# Run outstanding migrations
$ dotnet ef database update
```  
Connect to you SQL Server Instance using SSMS and check to confirm that the `Coupons` table was created in the `Mango_Coupon` database.  
To undo a migratiion
```bash
$ dotnet ef migrations remove
```
__Seeding__  
After the migration has generated the `Coupons` table, we need to seed the table.
First, update the `AppDbContext` class by overriding the `OnModelCreating` method of the base class. In the `OnModelCreating` method, the data to see the `Coupons` table will be implemented.  
After that, a new migration must be generate.
```bash
$ dotnet ef migrations add SedCouponTable
```
This will generate a migration file such as `Migrations/20251231090838_SedCouponTable.cs`.  

__Auto Migration__  
The `Program` file has been updated with the `ApplyMigration` method.  
This method is executed each time the application starts and it check for pending migrations a run migrations if any exists.  
After starting the application, you can check the database table to see if it has been seeded.  

__Auto Mapper__  
Auto Mapping has been implemented in the `MappingConfig` class file.   
Changes was done in the `Program.cs` file to integrate the Auto Mapper into the application service container.  

## Section 3: Coupon API - Crud
__Frontend__  
We start building the frontend of the application by adding a _ASP.NET Core Web App (MVC)_ project to the `Frontend` solution folder.    

__Newtonsoft.Json__  
The `Newtonsoft.Json` Nuget package is used to Serialize and Deserialize the request payload for API Request.  
You need to install the Package for the Web project.    

__Multiple Startup Projects__  
When we run the application, we want the _Web_ project as well as the _CouponAPI_ project to run  at the same time. To achive this, we need to configure the solution to have multiple startup projects:
- Right click on the solution > _Configure startup projects_
- Select _Multiple startup projects_
- Update all the project action to _"Start"_
- Click _Apply_ and _Ok_.
- Start the project to test it.
- All the startup pages should open in a browser window for each project.

__Theme and Bootstrap Icons__  
The Web project using the [Slate](https://bootswatch.com/slate) [Bootstrap theme](https://bootswatch.com/). You can download the CSS and include it in the `wwwroot/lib/bootstrap/dist/css` folder and update the `View/Shared/_Layout.cshtml` header section to use the style.  
For icon, we use the [BootStrap Icons](https://icons.getbootstrap.com/). The _CDN_ link to the CSS resource is also included in the head section of the `_Layout.cshtml` markup.  

__Toaster__  
We use the _toastr_ library to implement notification.  
The [CDN links](https://cdnjs.com/libraries/toastr.js) are included in a newly created `_Notification` partial which is the included in the _Layout_ shared view.  
The _Layout_ head section is also updated with the _toastr_ CSS library.  

## Section 4: Auth API
For the Auth API we add a new _ASP.Net Core Web API_ project named `Mango.Services.AuthAPI` to the `Services` solution folder.

__Dependencies__  
First, we copy all the dependencies from the `ItemGroup` section of the `CouponAPI`  project file (`.csproj` file)  to the  `AuthAPI` project file. This serves as the base dependencies.  
Next we install the Dotnet Identity package
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`

The Dotnet Identity package automatically creates all the identity related tables it needs using Entity Framework Core so it needs the `DbContext` to be implemented or it's use.

__Identity Setup__  
- First you need  to install the dependency `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- Next, you create `DbContext` class such as `Data/AppDbContext.cs`
- Update the request pipeline by including Identity services in `Program.cs`
```cs
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
...
app.UseHttpsRedirection();
app.UseAuthentication(); //Authentication must come before Authorization
app.UseAuthorization();
app.MapControllers();
ApplyMigration();
```
- Now you can run migration to generate the required Identity tables
```bash
$ cd Mango.Services.AuthAPI
$ dotnet ef migrations add addIdentityTables
```
This will generate the `Migrations/20260101104000_addIdentityTables.cs` which contains the code that create a bunch of tabled needed by  Dotnet Identity.
- Finally, we run the pending migration
```bash
$ cd Mango.Services.AuthAPI
$ dotnet ef database update
```
Check the `Mongo_Auth` database and you should see a couple of tables created by Dotnet Identity.  

__Extending the AspNetUsers table__
One of the tables created by Dotnet Identity is `AspNetUsers` which contain some fields for user information such as `Username` and `Email`.  We may want to extend this table by adding more fields to hold more user information.  
To do this, we must create a Models that extend the `IdentityUser` mode. See `Models/ApplicationUser.cs` for implementation.  
In the implementation, every place where `IdentityUser` is used must be replace with the `ApplicationUser`. This included the `Program.cs` and `Data/AppDbContext`.  
After the `Models/ApplicationUser.cs` is created and `Program.cs` and `Data/AppDbContext.cs` has been update, you can create a migration
```
$ dotnet ef migrations add AddNameToAspNetUsers
```

## Section 5: Consuming Auth API
__Signing in a user in .NET Identity__  
For the implementation we need to install the Nuget package:
- System.IdentityModel.Tokens.Jwt

## Section 6: Product API
Generate migration
```bash
$ cd Mango.Services.ProductAPI
$ dotnet ef migrations add AddProductsToDb
```

Run all outstanding migrations
```bash
$ dotnet ef database update
```

## Section 7: Hone Page and Details

## Section 8: Shopping Cart
Generate migration
```bash
$ cd Mango.Services.ShoppingCartAPI
$ dotnet ef  migrations add AddShoppingCartTables
```

Run all outstanding migrations
```bash
$ dotnet ef database update
```

# Section 9: Shopping Cart in Web Project
