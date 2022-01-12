using Luncher.Core.Entities;

namespace Luncher.Web.Models
{
    public record RestaurantVoteResponse(RestaurantType RestaurantType, ICollection<string> UserIds);
}
