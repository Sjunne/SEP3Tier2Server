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
        public async Task<ActionResult<IList<PrivateMessage>>> Get([FromQuery]string request)
        {
            var usernames = request.Split("|");
            Console.WriteLine(usernames[0]);
            Console.WriteLine(usernames[1]);

            Request request1 = new Request()
            {
                Username = usernames[0],
                o = usernames[1],
                requestOperation =  RequestOperationEnum.GETALLMESSAGES
            };
            
            IList<PrivateMessage> listOfMessages = _network.getAllPrivateMessages(request1);
            return Ok(listOfMessages);
        }

        
        
    }
}