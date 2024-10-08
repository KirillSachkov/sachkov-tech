using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Domain;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Application;

public interface IIssueReviewRepository
{
    Task<Result<IssueReview, Error>> GetById(IssueReviewId id, CancellationToken cancellationToken = default);
}

public class IssueReviewRepository : IIssueReviewRepository
{
    public Task<Result<IssueReview, Error>> GetById(IssueReviewId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}