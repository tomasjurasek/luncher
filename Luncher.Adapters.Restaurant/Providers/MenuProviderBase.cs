using Luncher.Core.Contracts;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant.Providers
{
    internal abstract class MenuProviderBase : IMenuProvider
    {
        public async Task<Menu> GetMenuAsync(RestaurantType restaurantType, CancellationToken cancellationToken)
        {
            try
            {
                return await GetMenuCoreAsync(restaurantType, cancellationToken);
            }
            catch (Exception)
            {
                //Log
                return Menu.Empty;
            }
        }

        protected abstract Task<Menu> GetMenuCoreAsync(RestaurantType restaurantType, CancellationToken cancellationToken);

        protected string GetExternalRestaurantId(RestaurantType restaurantType)
        {
            return restaurantType switch
            {
                RestaurantType.Padowetz => "2743-restaurant-padowetz",
                RestaurantType.Tusto => "2787-tusto-titanium",
                _ => throw new NotImplementedException(nameof(restaurantType))
            };
        }
    }
}
