using Luncher.Core.Entities;

namespace Luncher.Core.Contracts
{
    public interface IRestaurant
    {
        RestaurantType Type { get; }
        Task<Restaurant> GetInfoAsync(CancellationToken cancellationToken);
    }
}
