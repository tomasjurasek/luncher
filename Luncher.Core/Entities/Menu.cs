namespace Luncher.Core.Entities
{
    public class Menu
    {
        private static readonly Lazy<Menu> empty = new(() => new(Array.Empty<Meal>(), Array.Empty<Soap>()));

        public static Menu Create(ICollection<Meal> meals, ICollection<Soap> soaps) => new(meals, soaps);

        public static Menu Empty { get; } = empty.Value; //LazyThreadSafetyMode.ExecutionAndPublication

        public Menu(ICollection<Meal> meals, ICollection<Soap> soaps)
        {
            Meals = meals;
            Soaps = soaps;
        }

        public ICollection<Meal> Meals { get; }
        public ICollection<Soap> Soaps { get; }
    }
}
