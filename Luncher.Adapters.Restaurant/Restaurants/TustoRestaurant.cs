using Luncher.Adapters.Restaurant.MenuProviders;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant
{
    internal class TustoRestaurant : IRestaurant
    {
        public RestaurantType Type => RestaurantType.Tusto;

        private readonly IMenickaProvider _menuProvider;
        
        public TustoRestaurant(IMenickaProvider menuProvider)
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
