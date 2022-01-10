using Luncher.Web.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;

namespace Luncher.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IHubContext<VoteHub> _voteHub;
        private readonly IDistributedCache _cache;

        public VoteController(IHubContext<VoteHub> voteHub, IDistributedCache cache)
        {
            _voteHub = voteHub;
            _cache = cache;
        }

        [HttpPost("")]
        public IActionResult Vote([FromBody] VoteRequest request)
        {
            //TODO userId + storage
            var votesCount = _cache.GetString("votes:" + request.RestaurantId);
            if (votesCount is null)
            {
                _cache.SetString("votes:" + request.RestaurantId, 1.ToString());
            }
            else
            {
                var votes = int.Parse(votesCount) + 1;
                _cache.SetString("votes:" + request.RestaurantId, votes.ToString());
            }

            _voteHub.Clients.All.SendAsync("ReceiveVote", request.RestaurantId);

            return Ok();
        }

        [HttpGet("{restaurantId}")]
        public IActionResult GetVotes(string restaurantId)
        {
            var votesCount = _cache.GetString("votes:" + restaurantId);
            return Ok(votesCount);
        }

        public class VoteRequest
        {
            [JsonPropertyName("restaurantId")]
            public string RestaurantId { get; set; }

            [JsonPropertyName("userId")]
            public string UserId { get; set; }
        }
    }
}
