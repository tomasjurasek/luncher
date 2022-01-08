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
                foreach (var restaurant in _restaurants)
                {
                    var info = await restaurant.GetInfoAsync();
                    if (info is not null)
                    {
                        await _cache.SetStringAsync(info.Type.ToString(),
                      JsonSerializer.Serialize(info.MapToResponse()),
                      new DistributedCacheEntryOptions
                      {
                          AbsoluteExpiration = DateTime.UtcNow.AddHours(4)
                      });
                    }

                }

                await Task.Delay(TimeSpan.FromHours(4));
            }
        }
    }
}
