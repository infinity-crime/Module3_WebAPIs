using Tasks1and2_SimpleApiWithDI.Interfaces;
using Tasks1and2_SimpleApiWithDI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITimeService, TimeService>(); // зарегестрируем сервис в DI
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // Добавляем частичный класс Program для интеграционных тестов
