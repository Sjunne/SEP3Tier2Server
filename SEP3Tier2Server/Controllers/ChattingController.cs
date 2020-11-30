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
    public class ChattingController : ControllerBase
    {
        private INetwork _network;

        public ChattingController(INetwork network)
        {
            _network = network;
        }

        [HttpGet]
        public async Task<ActionResult<IList<PrivateMessage>>> Get([FromQuery]Request request)
        {
            IList<PrivateMessage> listOfMessages = _network.getAllPrivateMessages(request);
            return Ok(listOfMessages);
        }

        
        
    }
}