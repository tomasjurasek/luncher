using HtmlAgilityPack;
using Luncher.Core.Contracts;
using System.Text;

namespace Luncher.Adapters.Restaurant
{
    public class PadowetzRestaurant : IRestaurant
    {
        private readonly HtmlWeb _htmlWeb;
        private const string URL = "http://www.restaurant-padowetz.cz/poledni-menu.htm";

        public PadowetzRestaurant()
        {
            _htmlWeb = new HtmlWeb()
            {
                OverrideEncoding = Encoding.UTF8
            };
        }
        public async Task<Core.Entities.Restaurant> GetAsync()
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(URL);

            throw new NotImplementedException();
        }
    }
}
