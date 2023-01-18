using Luncher.Adapters.ThirdParty.Restaurants;
using Luncher.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Luncher.Adapters.ThirdParty.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurants(this IServiceCollection services)
        {
            //TODO use Scrutor
            services.AddSingleton<IRestaurant, AnnapurnaRestaurant>();
            services.AddSingleton<IRestaurant, PadowetzRestaurant>();
            services.AddSingleton<IRestaurant, TustoRestaurant>();
            services.AddSingleton<IRestaurant, CharliesRestaurant>();
            services.AddSingleton<IRestaurant, GardenRestaurant>();
            services.AddSingleton<IRestaurant, GrandKitchenRestaurant>();
            
            return services;
        }
    }
}
