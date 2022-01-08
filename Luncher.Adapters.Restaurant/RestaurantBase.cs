using Luncher.Core.Contracts;

namespace Luncher.Adapters.Restaurant
{
    internal abstract class RestaurantBase
    {
        protected Core.Entities.Type Type { get; }
        protected string Url { get; }

        public RestaurantBase(Core.Entities.Type type, string url)
        {
            Type = type;
            Url = url;
        }
    }
}
