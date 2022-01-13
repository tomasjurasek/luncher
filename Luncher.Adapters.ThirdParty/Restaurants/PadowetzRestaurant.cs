using HtmlAgilityPack;
using Luncher.Adapters.ThirdParty.Restaurants;
using Luncher.Core.Entities;
using Luncher.Domain.Entities;
using System.Text;

namespace Luncher.Adapters.ThirdParty
{
    internal class PadowetzRestaurant : RestaurantBase
    {
        private readonly HtmlWeb _htmlWeb;
        private string Url => $"http://www.restaurant-padowetz.cz/poledni-menu.htm";

        public PadowetzRestaurant() : base(RestaurantType.Padowetz)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "tydeniMenu ")
                .First();


            var soaps = todayMenuNode.Descendants("table")
                .Where(s => s.Attributes.Contains("id") && s.Attributes["id"].Value == "t_Polevky")
                .First()
                .Descendants("tr")
                .Select(s => $"{s.ChildNodes[0].InnerText} {s.ChildNodes[1].InnerText}")
                .Select(s => Soap.Create(s))
                .ToList();

            var meals = todayMenuNode.Descendants("table")
                .Where(s => s.Attributes.Contains("id") && s.Attributes["id"].Value == "t_Hlavni-chod")
                .First()
                .Descendants("tr")
                .Select(s => $"{s.ChildNodes[0].InnerText} {s.ChildNodes[1].InnerText}")
                .Select(s => Meal.Create(s))
                .ToList();

            return Restaurant.Create(Type, Menu.Create(meals, soaps));
        }
    }
}
