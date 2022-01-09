using HtmlAgilityPack;
using Luncher.Adapters.Restaurant.Utils;
using Luncher.Core.Entities;

namespace Luncher.Adapters.Restaurant.Restaurants
{
    internal class AnnapurnaRestaurant : RestaurantBase
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly HtmlWeb _htmlWeb;

        public AnnapurnaRestaurant(IDateTimeProvider dateTimeProvider) : base(Core.Entities.Type.Annapurna, "http://www.indicka-restaurace-annapurna.cz/")
        {
            _dateTimeProvider = dateTimeProvider;
            _htmlWeb = new HtmlWeb();
        }

        protected override async Task<Core.Entities.Restaurant> GetInfoCoreAsync(CancellationToken cancellationToken)
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url, cancellationToken);
            var today = _dateTimeProvider.GetToday();

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("p")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "TJden")
                .First(s => s.InnerText.Contains(today, StringComparison.InvariantCultureIgnoreCase))
                .NextSibling;

            //TODO Soaps

            var meals = todayMenuNode
                .Descendants("b")
                .Select(s => Meal.Create(s.InnerText))
                .ToList();

            return Core.Entities.Restaurant.Create(Type, Menu.Create(meals, Array.Empty<Soap>())); // TODO Soap Empty
        }
    }
}
