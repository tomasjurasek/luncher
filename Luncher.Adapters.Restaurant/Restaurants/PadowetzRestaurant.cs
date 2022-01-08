using HtmlAgilityPack;
using Luncher.Core.Entities;
using System.Text;

namespace Luncher.Adapters.Restaurant
{
    internal class PadowetzRestaurant : RestaurantBase
    {
        private readonly HtmlWeb _htmlWeb;

        public PadowetzRestaurant() : base(Core.Entities.Type.Padowetz, "https://www.menicka.cz/2743-restaurant-padowetz.html")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Core.Entities.Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "menicka")
                .First();

            var soaps = todayMenuNode.Descendants("li")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "polevka")
                .Select(s => s.Element("div")?.InnerText)
                .Where(s => s != null)
                .Select(s => Soap.Create(s))
                .ToList();

            var meals = todayMenuNode.Descendants("li")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "jidlo")
                .Select(s => s.Element("div")?.InnerText)
                .Where(s => s != null)
                .Select(s => Meal.Create(s))
                .ToList();

            return Core.Entities.Restaurant.Create(Type, Menu.Create(meals, soaps));
        }
    }
}
