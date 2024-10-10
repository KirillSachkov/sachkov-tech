using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Contracts;

public interface IIssuesReviewsContract
{
    Task<UnitResult<ErrorList>> AddComment(
        Guid issueReviewId,
        Guid userId,
        AddCommentRequest request,
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<ErrorList>> CreateIssueReview(
        Guid userIssueId,
        Guid userId,
        CreateIssueReviewRequest request,
        CancellationToken cancellationToken = default);
}