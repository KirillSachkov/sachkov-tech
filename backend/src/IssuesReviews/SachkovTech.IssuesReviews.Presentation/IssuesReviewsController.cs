using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.IssuesReviews.Application.Commands.StartReview;
using SachkovTech.IssuesReviews.Application.Queries.GetCommentsWithPagination;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Presentation;

public class IssuesReviewsController : ApplicationController
{
    private const string Sub = "sub";

    [HttpGet("{issueReviewId:guid}/comments")]
    public async Task<ActionResult> GetByIssueReviewId(
        [FromServices] GetCommentsWithPaginationHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromQuery] GetCommentsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .Handle(query.GetQueryWithId(issueReviewId), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/comment")]
    public async Task<ActionResult> Comment(
        [FromServices] AddCommentHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromBody] AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(Sub);

        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();

        var result = await handler.Handle(
            new AddCommentCommand(issueReviewId,
                Guid.Parse(userId),
                request.Message), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/start-review")]
    public async Task<ActionResult> StartReview(
        [FromServices] StartReviewHandler handler,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(Sub);

        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();

        var result = await handler.Handle(
            new StartReviewCommand(issueReviewId,
                Guid.Parse(userId)), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

}