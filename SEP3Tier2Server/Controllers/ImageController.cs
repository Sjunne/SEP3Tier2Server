using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<string> Get(string username)
        {
            Byte[] b = _network.GetCover(username);
            var base64 = Convert.ToBase64String(b);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            return imgSrc;
        }

        [HttpPost]
        public async Task<ActionResult<Request>> UploadImage([FromBody] Request request)
        {
            _network.UploadImage(request);
            return Created($"/added", request);
        }

        [HttpGet]
        [Route("All")]
        public async Task<string> GetPictures([FromQuery] string username)
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

            return allImages;
        }

    }
}