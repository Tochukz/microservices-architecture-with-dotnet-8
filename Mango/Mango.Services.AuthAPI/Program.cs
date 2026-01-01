using Microsoft.EntityFrameworkCore;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Mango.Services.AuthAPI.Service;
using Mango.Services.AuthAPI.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
//Dotnet Identity will use the DbContext to create all identitiy releated tables.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.GetPendingMigrations().Count() > 0)
        {
            dbContext.Database.Migrate();
        }
    }
}