using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using RBP.Web.Services;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Добавление автомапера
builder.Services.AddAutoMapper(typeof(MapperProfile));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHandbookService, HandbookService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IStatementService, StatementService>();

// Настройка культуры для приема чисел с точкой в качестве разделителя
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    List<CultureInfo> cultures = new() { new("en-US") };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

builder.Services.AddHttpClient();

// Add sessions ... (next more)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Cookie authentication ... (next more)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ... authentication + authorization setting
app.UseAuthentication();
app.UseAuthorization();

// ... sessions setting
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
