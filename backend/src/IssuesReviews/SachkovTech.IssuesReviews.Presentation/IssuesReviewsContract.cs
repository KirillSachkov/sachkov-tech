using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.IssuesReviews.Application.Commands.Create;
using SachkovTech.IssuesReviews.Contracts;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Presentation;

public class IssuesReviewsContract(
    AddCommentHandler addCommentHandler,
    CreateIssueReviewHandler createIssueReviewHandler) : IIssuesReviewsContract
{
    public async Task<UnitResult<ErrorList>> AddComment(
        Guid issueReviewId,
        Guid userId,
        AddCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        return await addCommentHandler.Handle(
            new AddCommentCommand(issueReviewId, userId, request.Message),
            cancellationToken);
    }

    public async Task<UnitResult<ErrorList>> CreateIssueReview(Guid userIssueId, Guid userId, CreateIssueReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateIssueReviewCommand(userIssueId, userId, request.PullRequestUrl);

        return await createIssueReviewHandler.Handle(command, cancellationToken);
    }
}