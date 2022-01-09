using Luncher.Core.Entities;

namespace Luncher.Core.Contracts
{
    public interface IMenuProvider
    {
        Task<Menu> GetMenuAsync(RestaurantType restaurantType, CancellationToken cancellationToken);
    }
}
