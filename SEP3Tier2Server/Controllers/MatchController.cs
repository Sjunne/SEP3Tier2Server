using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MainServerAPI.Data;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Mvc;

namespace MainServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private INetwork _network;

        public MatchController(INetwork network)
        {
            _network = network;
        }

        [HttpGet]
        public async Task<ActionResult<IList<String>>> getMatches([FromQuery] string username)
        {
            IList<String> usernames;
            try
            {
                usernames = _network.Matches(username);
                if (usernames == null)
                {
                    return StatusCode(503);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return Ok(usernames);
        }

        [HttpPost]
        [Route("Accept")]
        public async Task<ActionResult<Match>> AcceptMatch([FromBody] Match match)
        {
            RequestOperationEnum requestOperationEnum = _network.AcceptMatch(match);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                return StatusCode(503);
            }

            return Created($"/{match.usernames[0]}", match);

        }
        
        
        [HttpPost]
        [Route("Decline")]
        public async Task<ActionResult<int>> DeclineMatch([FromBody] Match match)
        {
          
            RequestOperationEnum requestOperationEnum = _network.DeclineMatch(match);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                
                return StatusCode(503);
            }
            
            return StatusCode(200);
        }
        
        [HttpPost]
        [Route("NewReview")]
        public async Task<ActionResult<Match>> AddNewReview([FromBody] Review review)
        {
            Request request = new Request()
            {
                Username = review.username,
                o = JsonSerializer.Serialize(review),
                requestOperation = RequestOperationEnum.ADDREVIEW
            };
            Request response = _network.AddReview(request);
            
            if (request.requestOperation == RequestOperationEnum.ERROR)
            {
                return StatusCode(503, response);
            }
            return Created($"/{request.Username}", response);

        }
        
        [HttpPatch]
        [Route("Report")]
        public async Task<ActionResult<Match>> ReportReview([FromBody] Request request)
        {
            Console.WriteLine("Hello");
         
            Request response = _network.ReportReview(request);
            
            if (request.requestOperation == RequestOperationEnum.ERROR)
            {
                return StatusCode(503, response);
            }
            return Ok(response);

        }
    }
}