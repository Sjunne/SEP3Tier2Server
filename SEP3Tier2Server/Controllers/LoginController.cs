using System;
using System.Text.Json;
using System.Threading.Tasks;
using MainServerAPI.Data;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SEP3Tier2Server.Exceptions;

namespace MainServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
         
        private INetwork _network;

        public LoginController(INetwork network)
        {
            _network = network;
        }
        
        [HttpGet]
        public async Task<ActionResult<string>> ValidateLogin([FromQuery] string username, string password)
        {
            try
            {
                Request request = _network.ValidateLogin(new User()
                {
                    username = username,
                    password = password
                });
                if (request.requestOperation == RequestOperationEnum.ERROR)
                {
                    return StatusCode(503, request.o);
                }
                
                return Ok(request);
            }
            catch (ServiceUnavailable e)
            {
                //kaster aldrig denne metode, hvad skal jeg så?
                return StatusCode(503, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Request>> RegisterUser ([FromBody] Request request)
        {
            Request response = _network.RegisterUser(request);
            //User user = JsonSerializer.Deserialize<User>(request.o.ToString());
            //response.o = user;
            if (request.requestOperation == RequestOperationEnum.ERROR)
            {
                return StatusCode(503, response);
            }
            return Created($"/{request.Username}", response);
        }

        [HttpPatch]
        public async Task<ActionResult<Request>> ChangePasswordOrUsername([FromBody] Request request)
        {
            Request response = _network.ChangePasswordOrUsername(request);
            if (request.requestOperation == RequestOperationEnum.ERROR)
            {
                return StatusCode(503, request.o);
            }
            return Ok(response);
        }
    }
}