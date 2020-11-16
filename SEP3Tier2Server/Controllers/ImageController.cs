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
            //Læser alle bytes fra et billedet og ligger det ind i et byte array
            
            Byte[] b = System.IO.File.ReadAllBytes(@"C:\Users\sjunn\RiderProjects\SEP3Tier2Server\SEP3Tier2Server\images\asdasd.jpg"); 
            //Returnere end File. Skal være JPG, ellers gemmer den som en "FILE" i stedet for
            return File(b, "image/jpg");
        }

    }
}