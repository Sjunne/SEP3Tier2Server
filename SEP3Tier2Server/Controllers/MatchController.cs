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
        public async Task<ActionResult<IList<String>>> getMatches([FromQuery] int userId)
        {
            IList<String> profilesId;
            try
            {
                profilesId = _network.Matches(userId);
                if (profilesId == null)
                {
                    return StatusCode(503);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return Ok(profilesId);
        }
    }
}