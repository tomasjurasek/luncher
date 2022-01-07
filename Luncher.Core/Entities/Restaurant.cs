namespace Luncher.Core.Entities
{
    public class Restaurant
    {
        public static Restaurant Create(string name, Menu menu) => new(name, menu);

        public Restaurant(string name, Menu menu)
        {
            Name = name;
            Menu = menu;
        }

        public string Name { get; }
        public Menu Menu { get; }
    }
}
