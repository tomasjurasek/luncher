using HtmlAgilityPack;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;
using System.Text.RegularExpressions;

namespace Luncher.Adapters.Restaurant.MenuProviders
{
    internal class AnnapurnaMenuProvider : MenuProviderBase, IAnnapurnaMenuProvider
    {
        private readonly HtmlWeb _htmlWeb;
        private string Url => $"http://www.indicka-restaurace-annapurna.cz/";

        public AnnapurnaMenuProvider()
        {
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Menu> GetMenuCoreAsync(RestaurantType restaurantType, CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("p")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "TJden")
                .First(s => s.InnerText.Contains(GetToday(), StringComparison.InvariantCultureIgnoreCase))
                .NextSibling;

            //TODO Soaps

            var meals = todayMenuNode
                .Descendants("b")
                .Select(s => Meal.Create(Regex.Replace(s.InnerText, @"^[0-9]\.", "")))
                .ToList();

            return Menu.Create(meals);
        }

        private string GetToday()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            var culture = new System.Globalization.CultureInfo("cs-CZ");
            return culture.DateTimeFormat.GetDayName(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone).DayOfWeek);
        }
    }

    internal interface IAnnapurnaMenuProvider : IMenuProvider
    {
    }
}
