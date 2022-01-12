using Luncher.Core.Entities;
using Luncher.Web.Hubs;
using Luncher.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Luncher.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IHubContext<VoteHub> _voteHub;
        private readonly IRestaurantFacade _restaurantFacade;

        public VoteController(IHubContext<VoteHub> voteHub, IRestaurantFacade restaurantFacade)
        {
            _voteHub = voteHub;
            _restaurantFacade = restaurantFacade;
        }

        [HttpPost("")]
        public async Task<IActionResult> Vote([FromBody] VoteRequest request)
        {
            var restaurantType = (RestaurantType)Enum.Parse(typeof(RestaurantType), request.RestaurantId);
            var result = await _restaurantFacade.SetVoteAsync(request.UserId, restaurantType);
            if(!result)
            {
                return BadRequest("The user already voted!");
            }

            await _voteHub.Clients.All.SendAsync("ReceiveVote", request.RestaurantId);
            return Ok();
        }

        [HttpGet("{userId}")]
        public IActionResult GetVotes([FromRoute] string userId)
        {
            return Ok(_restaurantFacade.GetVotedRestaurants(userId));
        }

        public class VoteRequest
        {
            [Required]
            [JsonPropertyName("restaurantId")]
            public string RestaurantId { get; set; }

            [Required]
            [JsonPropertyName("userId")]
            public string UserId { get; set; }
        }
    }
}
