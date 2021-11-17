using Luncher.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Luncher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRestaurant _restaurant;

        public HomeController(IRestaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public async Task<IActionResult> Index()
        {
            await _restaurant.GetAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}