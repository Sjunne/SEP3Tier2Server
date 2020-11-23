﻿using System;
 using System.Collections.Generic;
 using System.Threading.Tasks;
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
        [Route("Reviews")]
        public async Task<ActionResult<IList<Review>>> GetReviews([FromQuery] string username)
        {
            try
            {
                IList<Review> reviews = _network.GetReviews(username);
                return Ok(reviews);

            }
            catch (Exception e)
            {
                return StatusCode(503, e.Message);
            }
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
                return StatusCode(500, e.Message);
                
            }


            return Ok(profileData);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileData>> EditProfile([FromBody]ProfileData profileData)
        {
            _network.updateProfile(profileData);
            return Created($"/{profileData.username}", profileData);
        }
        
        [HttpPost]
        [Route("All")]
        public async Task<ActionResult<Request>> EditProfile([FromBody] Request request)
        {
            RequestOperationEnum requestOperationEnum = _network.editProfile(request);
            if (requestOperationEnum == RequestOperationEnum.SUCCESS)
                return Created($"/{request.Username}", request);
            else
                return StatusCode(503, requestOperationEnum);    
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