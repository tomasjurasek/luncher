namespace Luncher.Adapters.Restaurant
{
    internal abstract class RestaurantBase
    {
        protected string Name { get; }
        protected string Url { get; }

        public RestaurantBase(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
