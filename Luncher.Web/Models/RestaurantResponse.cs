namespace Luncher.Web.Models
{
    public class RestaurantResponse
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Food> Soaps { get; set; } = new List<Food>();    
        public ICollection<Food> Meals { get; set; } = new List<Food>();  
    }

    public class Food
    {
        public string Name { get; set; } = String.Empty;
    }

}
