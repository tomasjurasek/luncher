using Luncher.Adapters.Restaurant.MenuProviders;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant.Restaurants
{
    internal class AnnapurnaRestaurant : IRestaurant
    {
        public RestaurantType Type => RestaurantType.Annapurna;

        private readonly IAnnapurnaMenuProvider _menuProvider;

        public AnnapurnaRestaurant(IAnnapurnaMenuProvider menuProvider)
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
