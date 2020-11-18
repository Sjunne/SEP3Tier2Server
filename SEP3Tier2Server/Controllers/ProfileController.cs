﻿using System;
 using System.Text.Json;
 using System.Threading.Tasks;
using MainServerAPI.Data;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MainServerAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private INetwork _network;

        public ProfileController(INetwork network)
        {
            _network = network;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileData>> GetProfile([FromQuery] string username)
        {
            ProfileData profileData;
            try
            {
                profileData = _network.GetProfile(username);

                if (profileData == null)
                    return StatusCode(503);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(500, e);
            }


            return Ok(profileData);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileData>> AddProfile([FromBody]ProfileData profileData)
        {
            _network.updateProfile(profileData);
            return Created($"/{profileData.username}", profileData);
        }
        
        [Route("CreateProfile")]
        [HttpPost]
        public async Task CreateProfile([FromBody] ProfileData profileData)
        {
            Details self = JsonSerializer.Deserialize<Details>(profileData.jsonSelf);
            profileData.self = self;

            _network.CreateProfile(profileData);
        }
        

        [Route("CreatePreference")]
        [HttpPost]
        public async Task CreatePreference([FromBody] ProfileData profileData)
        {
            _network.CreatePreference(profileData);
        }
    }
}