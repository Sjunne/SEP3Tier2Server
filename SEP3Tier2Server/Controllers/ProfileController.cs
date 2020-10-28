﻿using System;
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
            ProfileData profileData = _network.GetProfile(username);
            return Ok(profileData);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileData>> AddProfile([FromBody]ProfileData profileData)
        {
            _network.updateProfile(profileData);
            return Created($"/{profileData.username}", profileData);
        }
    }
}