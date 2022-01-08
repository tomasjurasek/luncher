namespace Luncher.Web.Models
{
    public record RestaurantResponse(string Name, ICollection<Food> Soaps, ICollection<Food> Meals);

    public record Food(string Name);

}
