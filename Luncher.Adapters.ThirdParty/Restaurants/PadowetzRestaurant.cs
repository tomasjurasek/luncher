using Luncher.Adapters.ThirdParty.MenuProviders;
using Luncher.Core.Entities;
using Luncher.Domain.Contracts;

namespace Luncher.Adapters.ThirdParty
{
    internal class PadowetzRestaurant : IRestaurant
    {
        public RestaurantType Type => RestaurantType.Padowetz;

        private readonly IMenickaProvider _menuProvider;

        public PadowetzRestaurant(IMenickaProvider menuProvider)
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
