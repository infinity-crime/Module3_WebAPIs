using BooksKeeper.Domain.Entities;
using BooksKeeper.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.IntegrationInMemoryDbTests
{
    public class MyTestFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Удаляем существующие регистрации DbContext
                RemoveAllDbContextRegistrations(services);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDB");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                // Создаем новую БД для каждого теста
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                // Сеим тестовые данные
                SeedTestData(db);

                // Добавляем тестовую аутентификацию
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            }); 
        }

        private static void RemoveAllDbContextRegistrations(IServiceCollection services)
        {
            var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                                                  d.ServiceType == typeof(DbContextOptions) ||
                                                  d.ServiceType.Name.Contains("DbContext") ||
                                                  d.ServiceType == typeof(ApplicationDbContext)).ToList();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
        }

        private static void SeedTestData(ApplicationDbContext context)
        {
            context.Authors.Add(Author.Create("Jhon", "Mister"));
            context.Authors.Add(Author.Create("Victoria", "Secret"));
            context.SaveChanges();
        }
    }
}
