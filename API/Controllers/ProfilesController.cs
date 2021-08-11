using System.Threading.Tasks;
using Mediators;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username) {
            return await Process(new Profiles.Details{Username = username});
        }
    }
}