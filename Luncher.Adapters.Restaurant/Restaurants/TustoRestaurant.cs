using Luncher.Adapters.Restaurant.MenuProviders;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant
{
    internal class TustoRestaurant : RestaurantBase
    {
        private readonly IMenickaProvider _menuProvider;

        public TustoRestaurant(IMenickaProvider menuProvider) : base(RestaurantType.Tusto)
        {
            _menuProvider = menuProvider;
        }

        protected override async Task<Core.Entities.Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var menu = await _menuProvider.GetMenuAsync(Type, cancellationToken);

            return Core.Entities.Restaurant.Create(Type, menu);
        }
    }
}
