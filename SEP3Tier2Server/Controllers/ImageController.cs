using System;
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
            Console.WriteLine(imgSrc);
            string serialize = JsonSerializer.Serialize<string>(imgSrc);
            return serialize;
        }

    }
}