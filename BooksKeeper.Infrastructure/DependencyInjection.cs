using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Domain.Interfaces;
using BooksKeeper.Domain.Interfaces.Common;
using BooksKeeper.Infrastructure.BackgroundServices;
using BooksKeeper.Infrastructure.Caching;
using BooksKeeper.Infrastructure.Data;
using BooksKeeper.Infrastructure.Data.MongoDbRepositories;
using BooksKeeper.Infrastructure.Data.Repositories;
using BooksKeeper.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksKeeper.Domain.Entities.Identity;
using MassTransit;
using Orders.OrderWorkerService;
using Orders.NotificationService;
using Orders.InventoryService;

namespace BooksKeeper.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringDb = configuration.GetConnectionString("DefaultConnection");

            // Регистрация контекста базы данных с использованием Npgsql
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionStringDb);
            });

            // Регистрация репозиториев, UoW (для транзакций), сервиса кэширования
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookDapperRepository<BookYearCountResponse>, BookDapperRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, RedisDistributedCacheService>();
            services.AddScoped<IReviewMongoRepository, ProductReviewRepository>();

            // Регистрация Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Получение настроек Redis из конфигурации
            var redisConnectionString = configuration["Redis:RedisConnectionString"];
            var instanceName = configuration["Redis:InstanceName"];

            // Регистрация кэша Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = instanceName;
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = configuration["Redis:RedisConnectionString"];
                return ConnectionMultiplexer.Connect(redisConnection!);
            });

            // Регистрация клиента MongoDB
            var mongoConnectionString = configuration["Mongo:MongoConnectionString"];
            var mongoDatabaseName = configuration["Mongo:DatabaseName"];

            services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

            // Регистрация MassTransit
            var rabbitUsername = configuration["RabbitMqOptions:Username"];
            var rabbitPassword = configuration["RabbitMqOptions:Password"];

            services.AddMassTransit(options =>
            {
                // Настраиваем подключение к кролику
                options.UsingRabbitMq((context, config) =>
                {
                    config.Host("localhost", 5672, "/", cfg => 
                    { 
                        cfg.Username(rabbitUsername!); 
                        cfg.Password(rabbitPassword!); 
                    });

                    config.ConfigureEndpoints(context);

                    config.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(2)));
                });

                // добавляем слушателя, чтобы MassTransit автоматом создал очередь и привязал к ней его
                options.AddConsumer<SubmitOrderConsumer>();
                options.AddConsumer<NotificationServiceConsumer>();
                options.AddConsumer<InventoryServiceConsumer>();
            });

            // регистрация фонового сервиса расчета среднего отзыва книги
            services.AddHostedService<AverageRatingCalculatorService>();

            return services;
        }
    }
}
