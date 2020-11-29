﻿using System;
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
                return Ok(request);
            }
            catch (NetworkIssue e)
            {
                return StatusCode(404, e.Message);
            }
            catch (ServiceUnavailable e)
            {
                return StatusCode(503, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Request>> RegisterUser ([FromBody] Request request)
        {
            _network.RegisterUser(request);
            User user = JsonSerializer.Deserialize<User>(request.o.ToString());
            return Created($"/{request.Username}", user);
        }
    }
}