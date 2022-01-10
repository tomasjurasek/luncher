using Luncher.Adapters.ThirdParty.MenuProviders;
using Luncher.Core.Entities;
using Luncher.Domain.Contracts;

namespace Luncher.Adapters.ThirdParty.Restaurants
{
    internal class AnnapurnaRestaurant : IRestaurant
    {
        public RestaurantType Type => RestaurantType.Annapurna;

        private readonly IAnnapurnaMenuProvider _menuProvider;

        public AnnapurnaRestaurant(IAnnapurnaMenuProvider menuProvider)
        {
            _menuProvider = menuProvider;
        }

        public async Task<Domain.Entities.Restaurant> GetInfoAsync(CancellationToken cancellationToken)
        {
            var menu = await _menuProvider.GetMenuAsync(Type, cancellationToken);

            return Domain.Entities.Restaurant.Create(Type, menu);
        }
    }
}
