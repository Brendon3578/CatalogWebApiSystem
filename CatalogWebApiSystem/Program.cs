using CatalogWebApiSystem.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura��o de Servi�os (Startup -> ConfigureServices)

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogWebApiSystemContext>(options =>
    options.UseMySql(mySqlConnection,
    ServerVersion.AutoDetect(mySqlConnection)));


var app = builder.Build();

// Configura��o de Middlewares (Startup ->Configure)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    var scalarTitle = builder.Configuration["ScalarConfiguration:Title"] ?? "Catalog API";

    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference(configureOptions =>
    {
        configureOptions.WithTitle(scalarTitle);
        configureOptions.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
