using Luncher.Adapters.Restaurant;
using Luncher.Adapters.Restaurant.Restaurants;
using Luncher.Adapters.Restaurant.Utils;
using System.Threading.Tasks;
using Xunit;

namespace Luncher.Tests
{
    public class RestaurantTests
    {
        private readonly DateTimeProvider _dateTimeProvider;

        public RestaurantTests()
        {
            _dateTimeProvider = new DateTimeProvider();
        }

        [Fact]
        public async Task Annapurna()
        {
            var restaurant = new AnnapurnaRestaurant(_dateTimeProvider);
            var info = await restaurant.GetInfoAsync(default);

            Assert.NotNull(info);

        }

        [Fact]
        public async Task Tusto()
        {
            var restaurant = new TustoRestaurant();
            var info = await restaurant.GetInfoAsync(default);

            Assert.NotNull(info);

        }

        [Fact]
        public async Task Padowetz()
        {
            var restaurant = new PadowetzRestaurant();
            var info = await restaurant.GetInfoAsync(default);

            Assert.NotNull(info);

        }
    }
}