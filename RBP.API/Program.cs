using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RBP.API.Utils;
using RBP.Db;
using RBP.Db.Repositories;
using RBP.Services.Contracts;
using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Validators;
using RBP.Web.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Конфигурация авторизации по Bearer токену ... (ПРОДОЛЖЕНИЕ НИЖЕ!!!)
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

string secretKey = builder.Configuration.GetSection("JWTSettings:SecretKey").Value;
string issuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value;
string audience = builder.Configuration.GetSection("JWTSettings:Audience").Value;
SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true
    };
});

// Add services to the container.
builder.Services.AddControllers();

// Добавление автомапера
builder.Services.AddAutoMapper(typeof(MapperProfile));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHandbookRepository, HandbookRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStatisticRepository, StatisticRepository>();
builder.Services.AddScoped<IStatementRepository, StatementRepository>();
builder.Services.AddScoped<IValidator<Account>, AccountValidator>();
builder.Services.AddScoped<IValidator<AdminRoleData>, AdminRoleDataValidator>();
builder.Services.AddScoped<IValidator<EmployeeRoleData>, EmployeeRoleDataValidator>();
builder.Services.AddScoped<IHandbookValidator, HandbookValidator>();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
builder.Services.AddScoped<IValidator<Statement>, StatementValidator>();

// Отключение автоматической вылидации моделей в запросах
// builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// Добавление контекста базы данных
builder.Services.AddDbContext<PostgresContext>();

WebApplication app = builder.Build();

// app.UseHttpsRedirection();

// ... подключение авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();