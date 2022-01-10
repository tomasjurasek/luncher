using Luncher.Web.Services;

namespace Luncher.Web.BackgroundServices
{
    public class LoadRestaurantsBackgroundService : BackgroundService
    {
        private readonly IRestaurantFacade _restaurantService;

        public LoadRestaurantsBackgroundService(IRestaurantFacade restaurantService)
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
                    await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Do nothing. Shutting down.
                }

            }
        }
    }
}
