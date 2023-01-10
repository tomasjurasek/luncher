using HtmlAgilityPack;
using Luncher.Adapters.ThirdParty.Restaurants;
using Luncher.Core.Entities;
using Luncher.Domain.Entities;
using System.Text;
using System.Text.RegularExpressions;

namespace Luncher.Adapters.ThirdParty
{
    internal class GardenRestaurant : RestaurantBase
    {
        private readonly HtmlWeb _htmlWeb;
        private string Url => $"https://gardenpub.cz/poledni-menu";

        public GardenRestaurant() : base(RestaurantType.Garden)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "col-md-7")
                .First();

            var soaps = todayMenuNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "col-sm-8 col-md-9")
                .Select(s => s.InnerText)
                .Select(Soap.Create)
                .Take(2)
                .ToList();

            var meals = todayMenuNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "col-sm-8 col-md-9")
                .Select(s => Regex.Replace(s.InnerText, @"\s+", " "))
                .Select(Meal.Create)
                .TakeLast(5)
                .ToList();

            return Restaurant.Create(Type, Menu.Create(meals, soaps));
        }
    }
}