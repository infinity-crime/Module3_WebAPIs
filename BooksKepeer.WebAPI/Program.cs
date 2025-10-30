using BooksKeeper.Application;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Services;
using BooksKeeper.Infrastructure;
using BooksKepeer.WebAPI.Middleware;
using BooksKepeer.WebAPI.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using StackExchange.Redis;
using Swashbuckle.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ����������� �������� ���������� � �������������� � ������� extension �������
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ����������� FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();

// ������ ���� ������������ �� appsettings.json
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("BookPolicy", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(60));
    });
});

// ��������� ������������ Swagger
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

// ����������� health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// ���������� ��������������� ����������� Swagger
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ������������� ���������� middleware ��� ������ ������
app.UseMiddleware<ExceptionHandlerMiddleware>();

// ������������� ���������� middleware ��� ������ ���������� ��������
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

// �������� ����������� HTTP �������
app.UseOutputCache();

app.MapControllers();

// �������� ��� �������� �������� �������
app.MapHealthChecks("/healthz");

app.Run();
