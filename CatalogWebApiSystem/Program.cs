using CatalogWebApiSystem.Application.DTOs.Mappings;
using CatalogWebApiSystem.DataAccess.Context;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.DataAccess.Repositories;
using CatalogWebApiSystem.Extensions;
using CatalogWebApiSystem.Filters;
using CatalogWebApiSystem.Logging;
using Microsoft.EntityFrameworkCore;
using SwaggerThemes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Serviços (Startup -> ConfigureServices)

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogWebApiSystemContext>(options =>
    options.UseMySql(mySqlConnection,
    ServerVersion.AutoDetect(mySqlConnection)));

builder.Logging.AddProvider(new CustomLoggerProvider(
    new CustomLoggerProviderConfiguration { LogLevel = LogLevel.Information }
));

builder.Services.AddScoped<ApiLoggingResultFilter>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(ModelDTOMappingProfile));

var app = builder.Build();

// Configuração de Middlewares (Startup ->Configure)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiTitle = builder.Configuration["SwaggerConfiguration:Title"] ?? "Catalog API";
        var theme = @"/swagger/SwaggerDark.css";

        options.DocumentTitle = apiTitle;
        options.InjectStylesheet(theme);
    });

    app.ConfigureExceptionHandler();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
