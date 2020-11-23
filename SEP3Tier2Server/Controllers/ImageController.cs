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
                Byte[] b = _network.GetCover(username);
                var base64 = Convert.ToBase64String(b);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                return Ok(imgSrc);
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
                Byte[] b = _network.GetProfilePicture(username);
                var base64 = Convert.ToBase64String(b);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                return Ok(imgSrc);

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
            if(requestOperationEnum == RequestOperationEnum.SUCCESS)
                return Created($"/added", request);
           
            return StatusCode(503, requestOperationEnum);
        }

        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<string>> GetPictures([FromQuery] string username)
        {
            try
            { 
                List<Byte[]> b = _network.GetPictures(username);
                string allImages = "";
                for (int i = 0; i < b.Count; i++)
                {
                    string image = Convert.ToBase64String(b[i]);
                    string encoded = String.Format("data:image/gif;base64,{0}", image);
                    allImages += encoded;
                    allImages += "å";
                }

                return Ok(allImages);
            }
            catch (ServiceUnavailable e)
            {
                return StatusCode(503, e.Message);
            }

        }
        
        [HttpPost]
        [Route("UpdateCover")]
        public async Task<ActionResult> UpdateCover([FromBody]string pictureName)
        {
            RequestOperationEnum requestOperationEnum = _network.UpdateCover(pictureName);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                return StatusCode(503);
            }
            
            return StatusCode(200);
        }
        
        [HttpPost]
        [Route("UpdateProfilePic")]
        public async Task<ActionResult> UpdateProfilePic([FromBody]string pictureName)
        {
            RequestOperationEnum requestOperationEnum = _network.UpdateProfilePic(pictureName);
            if (requestOperationEnum == RequestOperationEnum.ERROR)
            {
                return StatusCode(503);
            }
            
            return StatusCode(200);
        }

    }
}