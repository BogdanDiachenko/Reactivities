using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.BusinessLogic.Profiles;

namespace Reactivities.Controllers;

public class ProfilesController : BaseApiController
{
    public ProfilesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await _mediator.Send(new Details.Query { Username = username }));
    }

    [HttpGet("{username}/activities")]
    public async Task<IActionResult> GetUserActivities(string username, string predicate)
    {
        return HandleResult(
            await _mediator.Send(new ListActivities.Query { Username = username, Predicate = predicate }));
    }
}