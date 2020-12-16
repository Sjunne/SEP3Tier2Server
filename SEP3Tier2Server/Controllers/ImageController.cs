using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEP3Tier2Server.Exceptions;

namespace MainServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        
            private INetwork _network;

            public ImageController(INetwork network)
            {
                _network = network;
            }

        [HttpGet]
        public async Task<ActionResult<string>> Get([FromQuery]string username)
        {
            //returns coverpicture for user
            try
            {
                return _network.GetCover(username);
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
        
        
        [HttpGet]
        [Route("ProfilePic")]
        public async Task<ActionResult<string>> GetProfilePic([FromQuery]string username)
        {

            try
            {
                return _network.GetProfilePicture(username);

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
        public async Task<ActionResult<Request>> UploadImage([FromBody] Request request)
        {
            RequestOperationEnum requestOperationEnum = _network.UploadImage(request);
            if (requestOperationEnum == RequestOperationEnum.SUCCESS)
            {
                return Created($"/added", request);
            }
           
            return StatusCode(503, requestOperationEnum);
        }

        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<string>> GetPictures([FromQuery] string username)
        {
            try
            {

                return _network.GetPictures(username);
            }
            catch (ServiceUnavailable e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(503, e.Message);
            }

        }

        

        [HttpPost]
        [Route("UpdateCover")]
        public async Task<ActionResult> UpdateCover([FromBody]Request request)
        {
            RequestOperationEnum requestOperationEnum = _network.UpdateCover(request);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                return StatusCode(503);
            }
            
            return StatusCode(200);
        }
        
        [HttpPost]
        [Route("UpdateProfilePic")]
        public async Task<ActionResult> UpdateProfilePic([FromBody]Request request)
        {
            RequestOperationEnum requestOperationEnum = _network.UpdateProfilePic(request);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                
                return StatusCode(503);
            }
            
            return StatusCode(200);
        }

        [HttpDelete]
        [Route("DeletePhoto/{pictureName}")]
        public async Task<ActionResult> DeletePhoto([FromRoute]string pictureName)
        {

            RequestOperationEnum requestOperationEnum = _network.DeletePhoto(pictureName);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                
                return StatusCode(503);
            }
            return Ok();
        }
    }
}