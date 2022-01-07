﻿using HtmlAgilityPack;
using Luncher.Core.Contracts;
using Luncher.Core.Entities;
using System.Text;

namespace Luncher.Adapters.Restaurant
{
    internal class TustoRestaurant : RestaurantBase, IRestaurant
    {
        private readonly HtmlWeb _htmlWeb;

        public TustoRestaurant() : base("Tusto", "https://www.menicka.cz/2787-tusto-titanium.html")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb();
        }

        public async Task<Core.Entities.Restaurant> GetInfoAsync()
        {
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(Url);

            var todayMenuNode = htmlDocument.DocumentNode.Descendants("div")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "menicka")
                .First();

            var soaps = todayMenuNode.Descendants("li")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "polevka")
                .Select(s => s.Element("div").InnerText)
                .Select(s => new Soap(s))
                .ToList();

            var meals = todayMenuNode.Descendants("li")
                .Where(s => s.Attributes.Contains("class") && s.Attributes["class"].Value == "jidlo")
                .Select(s => s.Element("div").InnerText)
                .Select(s => new Meal(s))
                .ToList();

            return new Core.Entities.Restaurant(Name, new Menu(meals, soaps));
        }
    }
}