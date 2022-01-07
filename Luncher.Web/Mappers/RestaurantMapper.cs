using Luncher.Core.Entities;
using Luncher.Web.Models;

namespace Luncher.Web.Mappers
{
    public static class RestaurantMapper
    {
        public static RestaurantResponse MapToResponse(this Restaurant restaurant)
        {
            return new RestaurantResponse
            {
                Name = restaurant.Type.ToString(),
                Soaps = restaurant.Menu.Soaps.Select(s => new Food { Name = s.Name }).ToArray(),
                Meals = restaurant.Menu.Meals.Select(s => new Food { Name = s.Name }).ToArray(),
            };
        }
    }
}
