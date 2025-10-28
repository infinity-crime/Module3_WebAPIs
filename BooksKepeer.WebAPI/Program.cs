using BooksKeeper.Application;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Services;
using BooksKeeper.Infrastructure;
using BooksKepeer.WebAPI.Middleware;
using BooksKepeer.WebAPI.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Npgsql;
using Swashbuckle.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов приложения и инфраструктуры с помощью extension методов
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Регистрация FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();

// Читаем нашу конфигурацию из appsettings.json
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// настройка документации Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Books Keeper API",
        Version = "v1",
        Description = "Simple REST-API made as an internship in Techcore.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Kirill Zhestkov",
            Email = "kirillzhestkov78@gmail.com"
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

    if(File.Exists(xmlPath))
    {
        o.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddHealthChecks();

//try 
//{
//    using var connection = new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));

//    connection.Open();
//    Console.WriteLine("Database connection is succes!");
//    connection.Close();
//}
//catch(Exception ex)
//{
//    Console.WriteLine($"Connection failed: {ex.Message}");
//}

var app = builder.Build();

// Подключаем непосредственно настроенный Swagger
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Использование кастомного middleware для отлова ошибок
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Использование кастомного middleware для замера выполнения запросов
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();
