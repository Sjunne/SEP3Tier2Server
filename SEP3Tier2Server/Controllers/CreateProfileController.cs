using System;
using System.Text.Json;
using System.Threading.Tasks;
using MainServerAPI.Data;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Mvc;

namespace MainServerAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CreateProfileController : ControllerBase
    {
        private INetwork _network;

        public CreateProfileController(INetwork network)
        {
            _network = network;
        }

        [HttpPost]
        public async Task CreateProfile([FromBody] ProfileData profileData)
        {
            Details self = JsonSerializer.Deserialize<Details>(profileData.jsonSelf);
            profileData.self = self;

            Details pref = JsonSerializer.Deserialize<Details>(profileData.jsonPref);
            profileData.preferences = pref;
            _network.CreateProfile(profileData);
        }
    }
}