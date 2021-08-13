using System.Threading.Tasks;
using DTO;
using Mediators;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return await Process(new Profiles.Details { Username = username });
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            return await Process(new Profiles.Update { Bio = profileDto.Bio, DisplayName = profileDto.DisplayName });
        }
    }
}