using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SachkovTech.Core.Abstractions;
using SachkovTech.IssuesReviews.Application;
using SachkovTech.IssuesReviews.Infrastructure.DbContexts;

namespace SachkovTech.IssuesReviews.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IssueReviewsWriteDbContext _dbContext;

    public UnitOfWork(IssueReviewsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}