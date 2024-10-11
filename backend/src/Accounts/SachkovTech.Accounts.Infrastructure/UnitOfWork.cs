using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace SachkovTech.Accounts.Infrastructure;

public class UnitOfWork
{
    private readonly AccountsDbContext _accountsDbContext;

    public UnitOfWork(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task<DbTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        var transaction = await _accountsDbContext.Database
            .BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }
}