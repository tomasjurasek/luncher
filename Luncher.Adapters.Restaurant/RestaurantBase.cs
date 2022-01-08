using Luncher.Core.Contracts;

namespace Luncher.Adapters.Restaurant
{
    internal abstract class RestaurantBase : IRestaurant
    {
        protected Core.Entities.Type Type { get; }
        protected string Url { get; }

        public RestaurantBase(Core.Entities.Type type, string url)
        {
            Type = type;
            Url = url;
        }

        public async Task<Core.Entities.Restaurant?> GetInfoAsync()
        {
            try
            {
                return await GetInfoCoreAsync();
            }
            catch (Exception ex)
            {
                //Log
                return null;
            }
        }

        protected abstract Task<Core.Entities.Restaurant>  GetInfoCoreAsync();
    }
}
