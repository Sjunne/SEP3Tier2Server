using System;
using System.Net;
using System.Net.Http;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {


        [HttpGet]
        public IActionResult Get()
        {            
            Byte[] b = System.IO.File.ReadAllBytes(@"C:\Users\sjunn\RiderProjects\SEP3Tier2Server\SEP3Tier2Server\images\asdasd.jpg");   // You can use your own method over here.         
            return File(b, "image/jpg");
        }

    }
}