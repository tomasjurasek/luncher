using HtmlAgilityPack;
using Luncher.Adapters.ThirdParty.Restaurants;
using Luncher.Core.Entities;
using Luncher.Domain.Entities;
using System.Text;
using System.Text.RegularExpressions;

namespace Luncher.Adapters.ThirdParty.Restaurants
{
    internal class GrandKitchenRestaurant : RestaurantBase
    {
        private readonly HtmlWeb _htmlWeb;
        private string Url => $"https://www.grandkitchenvlnena.cz/menu/";

        public GrandKitchenRestaurant() : base(RestaurantType.GrandKitchen)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("section")
                .Where(s => s.Attributes.Contains("class") &&
                            s.Attributes["class"].Value == "fly-dish-menu container-min jidel").ToList()[(int) DateTime.Today.DayOfWeek - 1 % 5 ];

            var soaps = todayMenuNode.Descendants("li")
                .Select(s => s.InnerText)
                .Select(Soap.Create)
                .Take(1)
                .ToList();

            var meals = todayMenuNode.Descendants("li")
                .Select(s => s.InnerText)
                .Select(Meal.Create)
                .TakeLast(4)
                .ToList();

            return Restaurant.Create(Type, Menu.Create(meals, soaps));
        }
    }
}