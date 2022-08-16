using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.BusinessLogic.Photos;

namespace Reactivities.Controllers;

public class PhotosController : BaseApiController
{
    public PhotosController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] Add.Command command)
    {
        return HandleResult(await _mediator.Send(command));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return HandleResult(await _mediator.Send(new Delete.Command{ Id = id}));
    }
    
    [HttpPost("{id}/setMain")]
    public async Task<IActionResult> SetMain(Guid id)
    {
        return HandleResult(await _mediator.Send(new SetMain.Command{ Id = id}));
    }
}