namespace Luncher.Domain.Entities
{
    public class Meal
    {
        public static Meal Create(string name) => new(name);

        public Meal(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
