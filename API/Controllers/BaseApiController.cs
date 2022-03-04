using System.Threading.Tasks;
using API.Extensions;
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

        protected async Task<ActionResult> Process<T>(IRequest<Result<PagedList<T>>> request)
        {
            return HandlePagedResult(await Mediator.Send(request));
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

        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result == null)
            {
                return NotFound();
            }
            
            if (result.IsSuccess)
            {
                if (result.Value == null) return NotFound();

                Response.AddPaginationHeader(result.Value.CurrentPage, 
                result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);
                
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}