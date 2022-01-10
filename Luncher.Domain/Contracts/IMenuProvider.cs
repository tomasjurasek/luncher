using Luncher.Core.Entities;
using Luncher.Domain.Entities;

namespace Luncher.Domain.Contracts
{
    public interface IMenuProvider
    {
        Task<Menu> GetMenuAsync(RestaurantType restaurantType, CancellationToken cancellationToken);
    }
}
