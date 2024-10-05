using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.IssuesReviews.Application.Queries.GetCommentsWithPagination;
using SachkovTech.IssuesReviews.Presentation.IssuesReviews.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Presentation.IssuesReviews;

public class IssuesReviewsController : ApplicationController
{
    private const string Sub = "sub";

    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromServices] GetCommentsWithPaginationHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromQuery] GetCommentsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .Handle(query.GetQueryWithId(issueReviewId), cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("{id:guid}")]
    public async Task<ActionResult> Create(
        [FromServices] AddCommentHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromBody] AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(Sub);

        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();
        
        var result = await handler.Handle(request
            .ToCommand(issueReviewId, Guid.Parse(userId)), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}