using HtmlAgilityPack;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;
using System.Text;

namespace Luncher.Adapters.Restaurant
{
    public class PadowetzRestaurant : IRestaurant
    {
        private readonly HtmlWeb _htmlWeb;
        private const string URL = "https://www.menicka.cz/2743-restaurant-padowetz.html";

        public PadowetzRestaurant()
        {
            _htmlWeb = new HtmlWeb();
        }
        public async Task<Core.Entities.Restaurant> GetAsync()
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(URL);

            var todayMenu = htmlDocument.DocumentNode.Descendants("div")
                .FirstOrDefault(s => s.Attributes["class"].Value == "menicka");

            return new Core.Entities.Restaurant("", new Menu(new List<Food>()));
        }
    }
}
