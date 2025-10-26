using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Services;
using BooksKepeer.WebAPI.Middleware;
using BooksKepeer.WebAPI.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookService, BookService>();

// ����������� FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();

// ������ ���� ������������ �� appsettings.json
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));


var app = builder.Build();

// ������������� ���������� middleware ��� ������ ������
app.UseMiddleware<ExceptionHandlerMiddleware>();

// ������������� ���������� middleware ��� ������ ���������� ��������
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
