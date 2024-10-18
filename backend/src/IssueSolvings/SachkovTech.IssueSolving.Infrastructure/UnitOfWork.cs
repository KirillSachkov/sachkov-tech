using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SachkovTech.Core.Abstractions;
using SachkovTech.IssueSolving.Application;
using SachkovTech.IssueSolving.Infrastructure.DbContexts;

namespace SachkovTech.IssueSolving.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IssueSolvingWriteDbContext _dbContext;

    public UnitOfWork(IssueSolvingWriteDbContext dbContext)
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