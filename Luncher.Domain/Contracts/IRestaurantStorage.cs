using Luncher.Core.Entities;
using Luncher.Domain.Entities;

namespace Luncher.Domain.Contracts
{
    public interface IRestaurantStorage
    {
        Task<ICollection<Restaurant>> GetAsync(CancellationToken cancellationToken);
        Task<Restaurant> FindAsync(RestaurantType restaurantType, CancellationToken cancellationToken);
        Task StoreAsync(Restaurant restaurant, CancellationToken cancellationToken);
    }
}
