using Luncher.Adapters.Restaurant.MenuProviders;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant
{
    internal class PadowetzRestaurant : IRestaurant
    {
        public RestaurantType Type => RestaurantType.Padowetz;

        private readonly IMenickaProvider _menuProvider;

        public PadowetzRestaurant(IMenickaProvider menuProvider)
        {
            _menuProvider = menuProvider;
        }

        public async Task<Core.Entities.Restaurant> GetInfoAsync(CancellationToken cancellationToken)
        {
            var menu = await _menuProvider.GetMenuAsync(Type, cancellationToken);

            return Core.Entities.Restaurant.Create(Type, menu);
        }
    }
}
