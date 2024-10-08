namespace SachkovTech.Accounts.Contracts;

public interface IAccountsContract
{
    // Task<UnitResult<ErrorList>> RegisterUser(
    //     RegisterUserRequest request, CancellationToken cancellationToken = default);
    //
    // Task SaveChanges(DbTransaction transaction, CancellationToken cancellationToken = default);

    Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
}