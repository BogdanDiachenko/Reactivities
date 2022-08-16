using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.BusinessLogic.Followers;

namespace Reactivities.Controllers;

public class FollowController : BaseApiController
{
    public FollowController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("{username}")]
    public async Task<IActionResult> Follow(string username)
    {
        return HandleResult(await _mediator.Send(new FollowToggle.Command { TargetUsername = username }));
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetFollowings(string username, bool requestingFollowers)
    {
        return HandleResult(await _mediator.Send(new List.Query
            { RequestingFollowers = requestingFollowers, Username = username }));
    }
}