using System.Data.Common;

namespace SachkovTech.IssueSolving.Application;

public interface IIssueSolvingUnitOfWork
{
    Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

    Task SaveChanges(CancellationToken cancellationToken = default, DbTransaction? transaction = null);
}