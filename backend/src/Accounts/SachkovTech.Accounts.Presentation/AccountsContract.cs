using SachkovTech.Accounts.Contracts;
using SachkovTech.Accounts.Infrastructure.IdentityManagers;

namespace SachkovTech.Accounts.Presentation;

public class AccountsContract : IAccountsContract
{
    private readonly PermissionManager _permissionManager;

    public AccountsContract(PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        return await _permissionManager.GetUserPermissionCodes(userId);
    }
}