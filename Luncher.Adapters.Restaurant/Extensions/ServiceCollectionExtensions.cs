using Luncher.Adapters.Restaurant.Restaurants;
using Luncher.Adapters.Restaurant.Utils;
using Luncher.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Luncher.Adapters.Restaurant.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurants(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddSingleton<IRestaurant, AnnapurnaRestaurant>();
            services.AddSingleton<IRestaurant, PadowetzRestaurant>();
            services.AddSingleton<IRestaurant, TustoRestaurant>();

            return services;
        }
    }
}
