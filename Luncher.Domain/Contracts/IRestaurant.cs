using Luncher.Core.Entities;
using Luncher.Domain.Entities;

namespace Luncher.Domain.Contracts
{
    public interface IRestaurant
    {
        RestaurantType Type { get; }
        Task<Restaurant> GetInfoAsync(CancellationToken cancellationToken);
    }
}
