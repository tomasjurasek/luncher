using Luncher.Core.Entities;
using Luncher.Domain.Contracts;
using Luncher.Web.Mappers;
using Luncher.Web.Models;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Luncher.Web.Services
{
    public class RestaurantFacade : IRestaurantFacade
    {
        private readonly IDistributedCache _cache;
        private readonly IEnumerable<IRestaurant> _restaurants;
        private readonly TelemetryClient _telemetryClient;

        private string GetRestaurantKey(RestaurantType restaurantType) => $"restaurant:{restaurantType}";
        private string GetRestaurantVotesKey(RestaurantType restaurantType) => $"votes:{restaurantType}";

        public RestaurantFacade(IDistributedCache cache, IEnumerable<IRestaurant> restaurants,
            TelemetryClient telemetryClient)
        {
            _cache = cache;
            _restaurants = restaurants;
            _telemetryClient = telemetryClient;
        }

        public async Task<ICollection<RestaurantResponse>> GetAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>()
                .Select(async s =>
                {
                    var restaurant = JsonSerializer.Deserialize<RestaurantResponse>(_cache.GetString(GetRestaurantKey(s)));
                    var restaurantType = (RestaurantType)Enum.Parse(typeof(RestaurantType), restaurant!.Name);
                    var restaurantVote = await GetVotesAsync(restaurantType);
                    return restaurant! with { Votes = restaurantVote.UserIds.Count };
                })
                .Select(s => s.Result);

            return restaurants.ToList();
        }

        public ICollection<string> GetVotedRestaurants(string userId)
        {
            var restaurantTypes = Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>()
                .Select(s =>
                   {
                       var restaurantVoteJson = _cache.GetString(GetRestaurantVotesKey(s));
                       if (restaurantVoteJson is not null)
                       {
                           var restaurantVotes = JsonSerializer.Deserialize<RestaurantVoteResponse>(restaurantVoteJson);
                           return restaurantVotes;
                       }

                       return new RestaurantVoteResponse(s, new List<string>());
                   })
               .Where(s => s!.UserIds.Contains(userId))
               .Select(s => s!.RestaurantType.ToString());

            return restaurantTypes.ToList();
        }

        public async Task<bool> SetVoteAsync(string userId, RestaurantType restaurantType, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetRestaurantVotesKey(restaurantType);
            var restaurantVoteJson = await _cache.GetStringAsync(cacheKey, cancellationToken);
            RestaurantVoteResponse restaurantVote;
            if (restaurantVoteJson is not null)
            {
                restaurantVote = JsonSerializer.Deserialize<RestaurantVoteResponse>(restaurantVoteJson);

                if (restaurantVote!.UserIds.Contains(userId))
                {
                    return false;
                }

                restaurantVote!.UserIds.Add(userId);
            }
            else
            {
                restaurantVote = new RestaurantVoteResponse(restaurantType, new List<string> { userId });
            }

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(restaurantVote), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(4)
            }, cancellationToken);

            _telemetryClient.TrackEvent("Vote", new Dictionary<string, string>
            {
                { "Restaurant", restaurantType.ToString() }
            });

            return true;
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

        private async Task<RestaurantVoteResponse> GetVotesAsync(RestaurantType restaurantType, CancellationToken cancellationToken = default) // TODO refactor
        {
            var cacheKey = GetRestaurantVotesKey(restaurantType);
            var restaurantVoteJson = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (restaurantVoteJson is null)
            {
                return new RestaurantVoteResponse(restaurantType, new List<string>());
            }

            return JsonSerializer.Deserialize<RestaurantVoteResponse>(restaurantVoteJson);
        }
    }

    public interface IRestaurantFacade
    {
        Task<ICollection<RestaurantResponse>> GetAsync(CancellationToken cancellationToken = default);
        Task ReloadAllAsync(CancellationToken cancellationToken = default);
        Task<bool> SetVoteAsync(string userId, RestaurantType restaurantType, CancellationToken cancellationToken = default);
        ICollection<string> GetVotedRestaurants(string userId);
    }
}
