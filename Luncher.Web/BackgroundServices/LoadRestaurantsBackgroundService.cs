using Luncher.Web.Services;

namespace Luncher.Web.BackgroundServices
{
    public class LoadRestaurantsBackgroundService : BackgroundService
    {
        private readonly IRestaurantFacade _restaurantFacade;

        public LoadRestaurantsBackgroundService(IRestaurantFacade restaurantFacade)
        {
            _restaurantFacade = restaurantFacade;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _restaurantFacade.ReloadAllAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Do nothing. Shutting down.
                }

            }
        }
    }
}
