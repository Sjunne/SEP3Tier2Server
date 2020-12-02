using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<int>> AcceptMatch([FromBody] IList<string> usernames)
        {
          
            RequestOperationEnum requestOperationEnum = _network.AcceptMatch(usernames);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                
                return StatusCode(503);
            }
            
            return StatusCode(200);
        }
    }
}