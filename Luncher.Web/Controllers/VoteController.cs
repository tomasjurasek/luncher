using Luncher.Core.Entities;
using Luncher.Web.Hubs;
using Luncher.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Luncher.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IHubContext<VoteHub> _voteHub;
        private readonly IRestaurantService _restaurantService;

        public VoteController(IHubContext<VoteHub> voteHub, IRestaurantService restaurantService)
        {
            _voteHub = voteHub;
            _restaurantService = restaurantService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Vote([FromBody] VoteRequest request)
        {
            var restaurantType = (RestaurantType)Enum.Parse(typeof(RestaurantType), request.RestaurantId);
            await _restaurantService.SetVoteAsync(restaurantType);
            await _voteHub.Clients.All.SendAsync("ReceiveVote", request.RestaurantId);

            return Ok();
        }

        public class VoteRequest
        {
            [Required]
            [JsonPropertyName("restaurantId")]
            public string RestaurantId { get; set; }

            [JsonPropertyName("userId")]
            public string UserId { get; set; }
        }
    }
}
