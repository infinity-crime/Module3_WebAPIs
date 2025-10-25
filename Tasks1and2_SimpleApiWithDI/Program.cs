using Tasks1and2_SimpleApiWithDI.Interfaces;
using Tasks1and2_SimpleApiWithDI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITimeService, TimeService>(); // �������������� ������ � DI
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
