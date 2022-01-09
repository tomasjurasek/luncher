using Luncher.Core.Contracts;
using Luncher.Web.Mappers;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Luncher.Web.BackgroundServices
{
    public class LoadRestaurantsBackgroundService : BackgroundService
    {
        private readonly IDistributedCache _cache;
        private readonly IEnumerable<IRestaurant> _restaurants;

        public LoadRestaurantsBackgroundService(IDistributedCache cache, IEnumerable<IRestaurant> restaurants)
        {
            _cache = cache;
            _restaurants = restaurants;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var restaurant in _restaurants)
                    {
                        var info = await restaurant.GetInfoAsync(stoppingToken);

                        await _cache.SetStringAsync(info.Type.ToString(),
                          JsonSerializer.Serialize(info.MapToResponse()),
                          new DistributedCacheEntryOptions
                          {
                              AbsoluteExpiration = DateTime.UtcNow.AddHours(4)
                          }, stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromHours(4), stoppingToken); // TODO Load only once in a day
                }
                catch (TaskCanceledException)
                {
                    // Do nothing. Shutting down.
                }

            }
        }
    }
}
