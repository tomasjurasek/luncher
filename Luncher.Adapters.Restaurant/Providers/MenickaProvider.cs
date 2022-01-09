using HtmlAgilityPack;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;
using System.Text;

namespace Luncher.Adapters.Restaurant.Providers
{
    internal class MenickaProvider : MenuProviderBase, IMenickaProvider
    {
        private readonly HtmlWeb _htmlWeb;
        private string Url(string restaurantId) => $"https://www.menicka.cz/{restaurantId}.html";
        public MenickaProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }
        protected override async Task<Menu> GetMenuCoreAsync(RestaurantType restaurantType, CancellationToken cancellationToken)
        {
            var externalId = GetExternalRestaurantId(restaurantType);
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url(externalId), cancellationToken);

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

            return Menu.Create(meals, soaps);
        }
    }

    internal interface IMenickaProvider : IMenuProvider
    {
    }
}
