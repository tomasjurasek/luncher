using Microsoft.AspNetCore.SignalR;

namespace Luncher.Web.Hubs
{
    public class VoteHub : Hub
    {
        public Task NotifyAsync(string restaurantId)
        {
            return Clients.All.SendAsync("ReceiveVote", restaurantId);
        }
    }
}
