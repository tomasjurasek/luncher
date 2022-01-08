using Luncher.Core.Contracts;

namespace Luncher.Adapters.Restaurant
{
    internal abstract class RestaurantBase : IRestaurant
    {
        protected Core.Entities.Type Type { get; }
        protected string Url { get; }

        public RestaurantBase(Core.Entities.Type type, string url) // TODO Adress
        {
            Type = type;
            Url = url;
        }

        public async Task<Core.Entities.Restaurant> GetInfoAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await GetInfoCoreAsync(cancellationToken);
            }
            catch (Exception)
            {
                //TODO Log
                return Core.Entities.Restaurant.Create(Type, Core.Entities.Menu.Empty);
            }
        }

        protected abstract Task<Core.Entities.Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken);
    }
}
