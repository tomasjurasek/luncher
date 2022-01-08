using System.Collections.ObjectModel;

namespace Luncher.Core.Entities
{
    public class Menu
    {
        private static readonly Lazy<Menu> _empty = new(() => new(Array.Empty<Meal>(), Array.Empty<Soap>()));

        public static Menu Create(IList<Meal> meals, IList<Soap> soaps) => new(meals, soaps);

        public static Menu Empty { get; } = _empty.Value; //LazyThreadSafetyMode.ExecutionAndPublication

        public Menu(IList<Meal> meals, IList<Soap> soaps)
        {
            Meals = new ReadOnlyCollection<Meal>(meals);
            Soaps = new ReadOnlyCollection<Soap>(soaps);
        }

        public IReadOnlyCollection<Meal> Meals { get; }
        public IReadOnlyCollection<Soap> Soaps { get; }
    }
}
