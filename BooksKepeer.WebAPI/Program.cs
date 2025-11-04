using BooksKeeper.Application;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.POCO.Settings;
using BooksKeeper.Application.Services;
using BooksKeeper.Infrastructure;
using BooksKepeer.WebAPI.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Swashbuckle.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов приложения и инфраструктуры с помощью extension методов
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Регистрация FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();

// Читаем нашу конфигурацию из appsettings.json
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("Mongo"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("BookPolicy", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(60));
    });
});

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

    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token in the format: Bearer {token}"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Регистрация health checks
builder.Services.AddHealthChecks();

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
app.UseRouting();

app.UseAuthentication(); // Добавляем аутентификацию
app.UseAuthorization();

// Включаем кэширование HTTP ответов
app.UseOutputCache();

app.MapControllers();

// Эндпоинт для проверки здоровья сервиса
app.MapHealthChecks("/healthz");

app.Run();
