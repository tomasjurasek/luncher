namespace Luncher.Core.Entities
{
    public class Menu
    {
        public static Menu Create(ICollection<Food> foods) => new(foods);

        public Menu(ICollection<Food> foods)
        {
            Foods = foods;
        }

        public ICollection<Food> Foods { get; }
    }
}
