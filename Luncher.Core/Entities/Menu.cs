namespace Luncher.Core.Entities
{
    public class Menu
    {
        public static Menu Create(ICollection<Meal> meals, ICollection<Soap> soaps) => new(meals, soaps);

        public Menu(ICollection<Meal> meals, ICollection<Soap> soaps)
        {
            Meals = meals;
            Soaps = soaps;
        }

        public ICollection<Meal> Meals { get; }
        public ICollection<Soap> Soaps { get; }
    }
}
