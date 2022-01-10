using Luncher.Core.Entities;
using Luncher.Domain.Contracts;
using Luncher.Web.Mappers;
using Luncher.Web.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Luncher.Web.Services
{
    public class RestaurantFacade : IRestaurantFacade
    {
        private readonly IDistributedCache _cache;
        private readonly IEnumerable<IRestaurant> _restaurants;

        private string GetRestaurantKey(RestaurantType restaurantType) => $"restaurant:{restaurantType}";
        private string GetRestaurantVoteKey(RestaurantType restaurantType) => $"votes:{restaurantType}";

        public RestaurantFacade(IDistributedCache cache, IEnumerable<IRestaurant> restaurants)
        {
            _cache = cache;
            _restaurants = restaurants;
        }

        public async Task<ICollection<RestaurantResponse>> GetAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>()
                .Select(async s =>
                {
                    var restaurant = JsonSerializer.Deserialize<RestaurantResponse>(_cache.GetString(GetRestaurantKey(s)));
                    var restaurantType = (RestaurantType)Enum.Parse(typeof(RestaurantType), restaurant!.Name);
                    var votes = await GetVotesAsync(restaurantType);
                    return restaurant! with { Votes = votes };
                })
                .Select(s => s.Result);

            return restaurants.ToList();
        }

        public async Task SetVoteAsync(RestaurantType restaurantType, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetRestaurantVoteKey(restaurantType);
            var cachedVotes = await _cache.GetStringAsync(cacheKey, cancellationToken);
            var votes = 1;
            if (cachedVotes is not null)
            {
                votes = int.Parse(cachedVotes) + 1;
            }

            await _cache.SetStringAsync(cacheKey, $"{votes}", new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(4)
            }, cancellationToken);
        }

        public async Task ReloadAllAsync(CancellationToken cancellationToken = default)
        {
            foreach (var restaurant in _restaurants)
            {
                var info = await restaurant.GetInfoAsync(cancellationToken);

                await _cache.SetStringAsync(GetRestaurantKey(info.Type),
                  JsonSerializer.Serialize(info.MapToResponse()),
                  new DistributedCacheEntryOptions
                  {
                      AbsoluteExpiration = DateTime.UtcNow.AddHours(2)
                  }, cancellationToken);
            }
        }

        private async Task<int> GetVotesAsync(RestaurantType restaurantType, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetRestaurantVoteKey(restaurantType);
            var votes = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (votes is null)
            {
                return 0;
            }

            return int.Parse(votes);
        }
    }

    public interface IRestaurantFacade
    {
        Task<ICollection<RestaurantResponse>> GetAsync(CancellationToken cancellationToken = default);
        Task ReloadAllAsync(CancellationToken cancellationToken = default);
        Task SetVoteAsync(RestaurantType restaurantType, CancellationToken cancellationToken = default);
    }
}
