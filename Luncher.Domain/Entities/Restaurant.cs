using Luncher.Core.Entities;

namespace Luncher.Domain.Entities
{
    public class Restaurant
    {
        public static Restaurant Create(RestaurantType type, Menu menu) => new(type, menu);

        public Restaurant(RestaurantType type, Menu menu)
        {
            Type = type;
            Menu = menu;
        }

        public RestaurantType Type { get; }
        public Menu Menu { get; }
    }
}
