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

        public async Task<IActionResult> Index()
        {
            var restaurants = Enum.GetValues(typeof(Core.Entities.Type)).Cast<Core.Entities.Type>()
                .Select(s => _cache.GetString(s.ToString()))
                .Where(s => s != null)
                .Select(s => JsonSerializer.Deserialize<RestaurantResponse>(s));

            return View(restaurants);
        }
    }
}