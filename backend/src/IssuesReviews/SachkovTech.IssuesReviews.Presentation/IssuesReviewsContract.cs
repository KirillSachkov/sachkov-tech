using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.IssuesReviews.Contracts;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Presentation;

public class IssuesReviewsContract(AddCommentHandler addCommentHandler) : IIssuesReviewsContract
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
}