using Luncher.Core.Entities;

namespace Luncher.Core.Contracts
{
    public interface IRestaurant
    {
        Task<Restaurant> GetInfoAsync();
    }
}
