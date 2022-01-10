﻿using Luncher.Core.Entities;
using Luncher.Domain.Contracts;
using Luncher.Web.Mappers;
using Luncher.Web.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Luncher.Web.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IDistributedCache _cache;
        private readonly IEnumerable<IRestaurant> _restaurants;

        private string GetRestaurantKey(RestaurantType restaurantType) => $"restaurant:{restaurantType}";
        private string GetRestaurantVoteKey(RestaurantType restaurantType) => $"votes:{restaurantType}";

        public RestaurantService(IDistributedCache cache, IEnumerable<IRestaurant> restaurants)
        {
            _cache = cache;
            _restaurants = restaurants;
        }

        public async Task<ICollection<RestaurantResponse>> GetAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>()
                .Select(s => _cache.GetString(GetRestaurantKey(s)))
                .Select(s => JsonSerializer.Deserialize<RestaurantResponse>(s));

            var response = new List<RestaurantResponse>(); // TODO ??
            foreach (var restaurant in restaurants)
            {
                var restaurantType = (RestaurantType)Enum.Parse(typeof(RestaurantType), restaurant.Name); // TODO refactor
                var votes = await GetVotesAsync(restaurantType);

                response.Add(restaurant! with { Votes = votes });
            }

            return response;
        }

        public async Task SetVoteAsync(RestaurantType restaurantType, CancellationToken cancellationToken = default) // TODO UserId
        {
            var cacheKey = GetRestaurantVoteKey(restaurantType);
            var votes = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (votes is null)
            {
                await _cache.SetStringAsync(cacheKey, $"{1}", cancellationToken);
            }
            else
            {
                await _cache.SetStringAsync(cacheKey, $"{int.Parse(votes) + 1}", cancellationToken);
            }
        }

        public async Task StoreAsync(CancellationToken cancellationToken = default) // TODO better name
        {
            foreach (var restaurant in _restaurants)
            {
                var info = await restaurant.GetInfoAsync(cancellationToken);

                await _cache.SetStringAsync(GetRestaurantKey(info.Type),
                  JsonSerializer.Serialize(info.MapToResponse()),
                  new DistributedCacheEntryOptions
                  {
                      AbsoluteExpiration = DateTime.UtcNow.AddHours(4)
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

    public interface IRestaurantService
    {
        Task<ICollection<RestaurantResponse>> GetAsync(CancellationToken cancellationToken = default);
        Task StoreAsync(CancellationToken cancellationToken = default);
        Task SetVoteAsync(RestaurantType restaurantType, CancellationToken cancellationToken = default);
    }
}