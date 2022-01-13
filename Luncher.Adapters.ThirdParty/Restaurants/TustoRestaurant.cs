using HtmlAgilityPack;
using Luncher.Adapters.ThirdParty.Restaurants;
using Luncher.Core.Entities;
using Luncher.Domain.Entities;
using System.Text;
using System.Text.RegularExpressions;

namespace Luncher.Adapters.ThirdParty
{
    internal class TustoRestaurant : RestaurantBase
    {
        private readonly HtmlWeb _htmlWeb;

        private string Url => $"https://www.menicka.cz/2787-tusto-titanium.html";

        public TustoRestaurant() : base(RestaurantType.Tusto)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "menicka")
                .First();


            var soaps = todayMenuNode.Descendants("li")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "polevka")
                .Select(s => s.ChildNodes[1].InnerText)
                .Select(s => Soap.Create(Regex.Replace(s, @"^[0-9]\.", "")))
                .ToList();

            var meals = todayMenuNode.Descendants("li")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "jidlo")
                .Select(s => s.ChildNodes[1].InnerText)
                .Select(s => Meal.Create(Regex.Replace(s, @"^[0-9]\.", "")))
                .ToList();

            return Restaurant.Create(Type, Menu.Create(meals, soaps));

        }
    }
}
