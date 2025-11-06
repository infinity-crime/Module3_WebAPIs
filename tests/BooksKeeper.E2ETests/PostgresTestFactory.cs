using BooksKeeper.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace BooksKeeper.E2ETests
{
    public class PostgresTestFactory : WebApplicationFactory<Program>, IDisposable
    {
        private readonly PostgreSqlContainer _postgreSqlContainer;

        public PostgresTestFactory()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithDatabase("testDb")
                .WithUsername("posgres")
                .WithPassword("postgres")
                .WithImage("postgres:16")
                .Build();

            // Запускаем контейнер синхронно
            _postgreSqlContainer.StartAsync().GetAwaiter().GetResult();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveAllDbContextRegistrations(services);

                var connectionString = _postgreSqlContainer.GetConnectionString();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.Migrate();
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

        public new void Dispose()
        {
            try
            {
                _postgreSqlContainer.StopAsync().GetAwaiter().GetResult();
                _postgreSqlContainer.DisposeAsync().GetAwaiter().GetResult();
            }
            catch
            {

            }

            base.Dispose();
        }
    }
}
