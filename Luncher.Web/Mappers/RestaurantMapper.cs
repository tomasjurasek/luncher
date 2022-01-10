using Luncher.Domain.Entities;
using Luncher.Web.Models;

namespace Luncher.Web.Mappers
{
    public static class RestaurantMapper
    {
        public static RestaurantResponse MapToResponse(this Restaurant restaurant)
        {
            return new RestaurantResponse(
                restaurant.Type.ToString(), 
                restaurant.Menu.Soaps.Select(s => new Food(s.Name)).ToArray(), 
                restaurant.Menu.Meals.Select(s => new Food(s.Name)).ToArray(),
                0);

        }
    }
}
