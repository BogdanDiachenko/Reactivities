using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Core.Helpers.Paging;
using Reactivities.Core.Models;
using Reactivities.Extensions;

namespace Reactivities.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected ActionResult HandleResult<TValue>(ServiceResult<TValue> result)
    {
        if (result == null || result.Value == null)
        {
            return NotFound();
        }
        
        if (!result.Success)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
    
    protected ActionResult HandlePagedResult<TValue>(ServiceResult<PagedList<TValue>> result)
    {
        if (result == null || result.Value == null)
        {
            return NotFound();
        }
        
        if (!result.Success)
        {
            return BadRequest(result.Error);
        }

        Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize, 
            result.Value.TotalCount, result.Value.TotalPages);
        return Ok(result.Value);
    }
}