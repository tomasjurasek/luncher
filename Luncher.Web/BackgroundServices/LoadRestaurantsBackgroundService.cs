using Luncher.Web.Services;

namespace Luncher.Web.BackgroundServices
{
    public class LoadRestaurantsBackgroundService : BackgroundService
    {
        private readonly IRestaurantService _restaurantService;

        public LoadRestaurantsBackgroundService(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _restaurantService.ReloadAllAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromHours(4), stoppingToken); // TODO Load only once in a day
                }
                catch (TaskCanceledException)
                {
                    // Do nothing. Shutting down.
                }

            }
        }
    }
}
