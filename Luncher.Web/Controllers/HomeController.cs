using Luncher.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Luncher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache _cache;

        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [ResponseCache(Duration = 60)]
        public IActionResult Index()
        {
            var restaurants = Enum.GetValues(typeof(Core.Entities.RestaurantType)).Cast<Core.Entities.RestaurantType>()
                .Select(s => _cache.GetString(s.ToString()))
                .Select(s => JsonSerializer.Deserialize<RestaurantResponse>(s));

            return View(restaurants);
        }
    }
}