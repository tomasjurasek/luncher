using Luncher.Core.Entities;
using Luncher.Domain.Entities;

namespace Luncher.Adapters.ThirdParty.Restaurants
{
    internal class SaigonRestaurant : RestaurantBase
    {
        public SaigonRestaurant() : base(RestaurantType.Saigon)
        {
        }

        protected override Task<Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
