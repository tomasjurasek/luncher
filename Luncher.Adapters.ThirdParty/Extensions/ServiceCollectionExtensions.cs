using Luncher.Adapters.ThirdParty.Restaurants;
using Luncher.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Luncher.Adapters.ThirdParty.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurants(this IServiceCollection services)
        {
            services.AddSingleton<IRestaurant, AnnapurnaRestaurant>();
            services.AddSingleton<IRestaurant, PadowetzRestaurant>();
            services.AddSingleton<IRestaurant, TustoRestaurant>();

            return services;
        }
    }
}
