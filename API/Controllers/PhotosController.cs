using System.Threading.Tasks;
using Mediators;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Photos.Add request)
        {
            return await Process(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await Process(new Photos.Delete { Id = id });
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return await Process(new Photos.SetMain { Id = id });
        }
    }
}