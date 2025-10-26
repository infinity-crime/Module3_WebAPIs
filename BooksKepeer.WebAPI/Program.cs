using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookService, BookService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
