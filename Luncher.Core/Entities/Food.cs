namespace Luncher.Core.Entities
{
    public class Food
    {
        public static Food Create(string name) => new(name);

        public Food(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
