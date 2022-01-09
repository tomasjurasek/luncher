using Luncher.Core.Contracts;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant
{
    internal abstract class RestaurantBase : IRestaurant
    {
        protected RestaurantType Type { get; }

        public RestaurantBase(RestaurantType type) // TODO Address
        {
            Type = type;
        }

        public async Task<Core.Entities.Restaurant> GetInfoAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await GetInfoCoreAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                //TODO Log
                return Core.Entities.Restaurant.Create(Type, Core.Entities.Menu.Empty);
            }
        }

        protected abstract Task<Core.Entities.Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken);
    }
}
