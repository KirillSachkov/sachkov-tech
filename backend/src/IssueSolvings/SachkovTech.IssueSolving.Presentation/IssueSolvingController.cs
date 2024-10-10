using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.IssueSolving.Application.Commands.TakeOnWork;

namespace SachkovTech.IssueSolving.Presentation;

public class IssueSolvingController : ApplicationController
{
    [Authorize]
    [HttpPost("{issueId:guid}")]
    public async Task<ActionResult> TakeOnWork(
        [FromRoute] Guid issueId,
        [FromServices] TakeOnWorkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "sub").Value;
        
        var command = new TakeOnWorkCommand(Guid.Parse(userId), issueId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}