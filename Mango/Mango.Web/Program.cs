using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Registers IHttpContextAccessor in the DI container so that you can have IHttpContextAccessor in your class constructor
builder.Services.AddHttpClient(); //Registers the HttpClientFactory in the DI container so that you can have IHttpClientFactory or HttpClient in your class constructor 
builder.Services.AddHttpClient<ICouponService, CouponService>(); // Registers a typed HttpClient. A dedicated HttpClient instance is injected into CouponService. That HttpClient is managed by HttpClientFactory
builder.Services.AddHttpClient<IAuthService, AuthService>();

StyleDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
StyleDetails.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
