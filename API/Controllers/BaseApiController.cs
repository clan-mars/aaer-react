using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator mediator;
        private IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected async Task<ActionResult> Process<T>(IRequest<Result<T>> request)
        {
            return HandleResult(await Mediator.Send(request));
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
            {
                return NotFound();
            }
            if (result.IsSuccess)
            {
                return result.Value == null ? NotFound() : Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}