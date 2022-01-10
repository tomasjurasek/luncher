using Luncher.Core.Entities;
using Luncher.Web.Models;
using Luncher.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Luncher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public HomeController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {
            var restaurants = await _restaurantService.GetAsync();

            return View(restaurants);
        }
    }
}