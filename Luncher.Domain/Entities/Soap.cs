namespace Luncher.Domain.Entities
{
    public class Soap
    {
        public static Soap Create(string name) => new(name);

        public Soap(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
