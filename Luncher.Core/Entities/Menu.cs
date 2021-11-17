namespace Luncher.Core.Entities
{
    public class Menu
    {
        public Menu(ICollection<Food> foods)
        {
            Foods = foods;
        }

        public ICollection<Food> Foods { get; }
    }
}
