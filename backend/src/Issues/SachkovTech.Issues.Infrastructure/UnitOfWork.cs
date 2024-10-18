using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IssuesWriteDbContext _dbContext;

    public UnitOfWork(IssuesWriteDbContext dbContext)
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