using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.POCO.Settings;
using BooksKeeper.Application.Services;
using BooksKeeper.Domain.Interfaces;
using BooksKeeper.Infrastructure;
using BooksKeeper.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("Mongo"));

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
