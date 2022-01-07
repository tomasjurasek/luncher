namespace Luncher.Core.Entities
{
    public class Restaurant
    {
        public static Restaurant Create(Type type, Menu menu) => new(type, menu);

        public Restaurant(Type type, Menu menu)
        {
            Type = type;
            Menu = menu;
        }

        public Type Type { get; }
        public Menu Menu { get; }
    }
}
