using System.Data.Common;

namespace SachkovTech.Issues.Application
{
    public interface IUnitOfWork
    {
        Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

        Task SaveChanges(CancellationToken cancellationToken = default, DbTransaction? transaction = null);
    }
}
