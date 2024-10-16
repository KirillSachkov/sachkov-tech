using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.IssueSolving.Application.Commands.SendOnReview;
using SachkovTech.IssueSolving.Application.Commands.TakeOnWork;
using SachkovTech.IssueSolving.Contracts.Requests;

namespace SachkovTech.IssueSolving.Presentation;

public class IssueSolvingController : ApplicationController
{
    //[Authorize]
    [HttpPost("{issueId:guid}")]
    public async Task<ActionResult> TakeOnWork(
        [FromRoute] Guid issueId,
        [FromServices] TakeOnWorkHandler handler,
        CancellationToken cancellationToken = default)
    {
        //var userId = HttpContext.User.Claims.First(c => c.Type == "sub").Value;

        var userId = "b5e92402-28d4-4f9b-9272-0fa5fad90976";

        var command = new TakeOnWorkCommand(Guid.Parse(userId), issueId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    //[Authorize]
    [HttpPost("{userIssueId:guid}/review")]
    public async Task<ActionResult> SendOnReview(
        [FromRoute] Guid userIssueId,
        [FromServices] SendOnReviewHandler handler,
        [FromBody] SendOnReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        //var userId = HttpContext.User.Claims.First(c => c.Type == "sub").Value;

        var command = new SendOnReviewCommand(userIssueId, Guid.Parse("b5e92402-28d4-4f9b-9272-0fa5fad90976"), request.PullRequestUrl);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}