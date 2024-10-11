using System.Data.Common;

namespace SachkovTech.IssuesReviews.Application;

public interface IIssuesReviewsUnitOfWork
{
    Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

    Task SaveChanges(CancellationToken cancellationToken = default, DbTransaction? transaction = null);
}