using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.BusinessLogic.Activities;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers.Paging;
using Reactivities.Core.Models;
using Reactivities.Security;

namespace Reactivities.Controllers;

[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ActivitiesController : BaseApiController
{
    public ActivitiesController(IMediator mediator) : base(mediator)
    {
    }
    
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    [HttpGet]
    public async Task<IActionResult> GetActivities([FromQuery] ActivityParams param)
    {
        return HandlePagedResult(await _mediator.Send(new List.Query{Params = param}));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(Guid id)
    {
        return HandleResult(await _mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity)
    {
        return HandleResult(await _mediator.Send(new Create.Command { Activity = activity}));
    }
    
    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Activity activity)
    {
        return HandleResult(await _mediator.Send(new Edit.Command { Activity = activity}));
    }
    
    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        return HandleResult(await _mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        return HandleResult(await _mediator.Send(new UpdateAttendance.Command { Id = id }));
    }
}
