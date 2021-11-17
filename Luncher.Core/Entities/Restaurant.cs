namespace Luncher.Core.Entities
{
    public class Restaurant
    {
        public Restaurant(string name, Menu menu)
        {
            Name = name;
            Menu = menu;
        }

        public string Name { get; }
        public Menu Menu { get; }
    }
}
