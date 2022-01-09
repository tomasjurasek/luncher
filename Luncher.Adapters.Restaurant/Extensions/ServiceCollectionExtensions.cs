using Luncher.Adapters.Restaurant.MenuProviders;
using Luncher.Adapters.Restaurant.Restaurants;
using Luncher.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Luncher.Adapters.Restaurant.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurants(this IServiceCollection services)
        {
            services.AddSingleton<IRestaurant, AnnapurnaRestaurant>();
            services.AddSingleton<IRestaurant, PadowetzRestaurant>();
            services.AddSingleton<IRestaurant, TustoRestaurant>();

            services.AddSingleton<IMenickaProvider, MenickaProvider>();

            return services;
        }
    }
}
