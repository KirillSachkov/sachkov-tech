﻿using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SachkovTech.Core.Abstractions;
using SachkovTech.IssuesReviews.Application;
using SachkovTech.IssuesReviews.Infrastructure.DbContexts;

namespace SachkovTech.IssuesReviews.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _dbContext;

    public UnitOfWork(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default, DbTransaction? dbTransaction = null)
    {
        await _dbContext.Database.UseTransactionAsync(dbTransaction, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}