using Luncher.Core.Contracts;
using Luncher.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Luncher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IRestaurant> _restaurants;

        public HomeController(IEnumerable<IRestaurant> restaurants)
        {
            _restaurants = restaurants;
        }

        public async Task<IActionResult> Index()
        {
            var restaurants = await Task.WhenAll(_restaurants.Select(s => s.GetInfoAsync()));
            return View(restaurants);
        }
    }
}