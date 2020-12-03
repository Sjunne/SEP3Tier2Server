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
                Console.WriteLine(e.Message + "123");
                return StatusCode(500, e.Message);
            }
            
            return Ok(profileData);
        }
        

        [HttpPost]
        public async Task<ActionResult<ProfileData>> EditProfile([FromBody]ProfileData profileData)
        {
            RequestOperationEnum requestOperationEnum = _network.updateProfile(profileData);
            if(requestOperationEnum == RequestOperationEnum.SUCCESS)
               return Created($"/{profileData.username}", profileData);
            
            return StatusCode(503);
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

        [HttpDelete]
        [Route("delete")]
        public async Task deleteProfile([FromQuery] string username)
        {
            _network.deleteProfile(username);
        }
        [HttpPost]
        [Route("bigEditProfile")]
        public async Task<ActionResult<Request>> bigEditProfile([FromBody] ProfileData profileData)
        {
            try
            {
                
                Console.WriteLine(profileData.preferences.nationality);
                Console.WriteLine(profileData.username);

                if (profileData == null)
                    return StatusCode(503);
                
                profileData.self= JsonSerializer.Deserialize<Details>(profileData.jsonSelf);
                _network.bigEditProfile(profileData);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
                
            }


            return Ok(profileData);    
        }
        [HttpPost]
        [Route("EditPreference")]
        public async Task<ActionResult<Request>> EditPreference([FromBody] ProfileData profileData)
        {
            try
            {
                
                Console.WriteLine(profileData.preferences.nationality);
                Console.WriteLine(profileData.username);

                if (profileData == null)
                    return StatusCode(503);
                
                profileData.preferences= JsonSerializer.Deserialize<Details>(profileData.jsonPref);
                _network.editPreference(profileData);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
                
            }


            return Ok(profileData);    
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
        
        [HttpGet]
        [Route("Preference")]
        public async Task<ActionResult<ProfileData>> GetPreference([FromQuery] string username)
        {
            ProfileData profileData;
            try
            {
                profileData = _network.GetPreference(username);
                Console.WriteLine(profileData.preferences.nationality);
                Console.WriteLine(profileData.username);

                if (profileData == null)
                    return StatusCode(503);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
                
            }


            return Ok(profileData);
        }
    }
}