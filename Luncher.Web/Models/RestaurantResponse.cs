namespace Luncher.Web.Models
{
    public class RestaurantResponse
    {
        public string Name { get; set; }
        public ICollection<Food> Soaps { get; set; }
        public ICollection<Food> Meal { get; set; }
    }

    public class Food
    {
        public string Name { get; set; }
    }

}
