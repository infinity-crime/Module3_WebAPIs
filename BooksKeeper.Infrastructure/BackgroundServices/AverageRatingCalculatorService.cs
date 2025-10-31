using BooksKeeper.Application.POCO.Settings;
using BooksKeeper.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BooksKeeper.Infrastructure.BackgroundServices
{
    public class AverageRatingCalculatorService : BackgroundService
    {
        private readonly ILogger<AverageRatingCalculatorService> _logger;
        private readonly IMongoClient _mongoClient;
        private readonly IConnectionMultiplexer _redis;

        private readonly MongoDbSettings _mongoSettings;

        private readonly TimeSpan _period = TimeSpan.FromMinutes(5);

        public AverageRatingCalculatorService(ILogger<AverageRatingCalculatorService> logger, 
            IOptions<MongoDbSettings> options, 
            IMongoClient mongoClient,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _mongoSettings = options.Value;
            _mongoClient = mongoClient;
            _redis = connectionMultiplexer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AverageRatingCalculatorService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var redis = _redis.GetDatabase();

                    var database = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
                    var collection = database.GetCollection<ProductReview>("reviews");

                    /*
                        Метод Aggregate() используется для выполнения агрегации на стороне MongoDB. 
                        Использование этого метода не выполняет загрузку всех данных в память приложения. А лишь
                        подготавливает выстраивание конвейера агрегации, который затем выполняется на сервере базы данных.

                        Group() - этап, который группирует документы по полю BookId. Для каждой группы вычисляются:
                        Avg - среднее значение поля Rating в группе.
                        Count - количество документов в группе.

                        А вот ToListAsync() уже выполняет саму агрегацию на сервере MongoDB и возвращает результат в виде списка.
                    */

                    var pipeline = collection.Aggregate()
                        .Group(r => r.BookId, g => new AvgDto( 
                            g.Key, 
                            g.Average(x => x.Rating), 
                            g.Count() 
                        ));

                    var list = await pipeline.ToListAsync(stoppingToken);
                    foreach (var item in list)
                    {
                        var redisKey = $"rating:{item.BookId}";

                        var avgStr = item.Average.ToString("F2", CultureInfo.InvariantCulture);

                        await redis.StringSetAsync(redisKey, avgStr);

                        _logger.LogInformation($"Updated {redisKey} = {avgStr} (count = {item.Count})");
                    }

                    _logger.LogInformation("Average ratings calculation completed.");
                }
                catch(OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("AverageRatingCalculatorService stoped");

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while calculating/storing average ratings. Will retry after delay.");
                }
            }
        }
    }

    public record AvgDto(Guid BookId, double Average, int Count);
}
