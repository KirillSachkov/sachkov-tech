using System.Data.Common;
using CSharpFunctionalExtensions;
using SachkovTech.Accounts.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Contracts;

public interface IAccountsContract
{
    Task<UnitResult<ErrorList>> RegisterUser(
        RegisterUserRequest request, CancellationToken cancellationToken = default);

    Task SaveChanges(DbTransaction transaction, CancellationToken cancellationToken = default);
}