using System.Threading.Tasks;
using Mediators;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> FollowUser(string username)
        {
            return await Process(new Followings.Follow { TargetName = username });
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> UnfollowUser(string username)
        {
            return await Process(new Followings.Unfollow { TargetName = username });
        }

        
    }
}