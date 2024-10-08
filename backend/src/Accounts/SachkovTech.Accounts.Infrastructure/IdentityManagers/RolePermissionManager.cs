using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager(AccountsDbContext accountsContext)
{
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
            if (permission == null)
                throw new ApplicationException($"Permission code {permissionCode} is not found.");

            var rolePermissionExist = await accountsContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

            if (rolePermissionExist)
                continue;

            accountsContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });
        }

        await accountsContext.SaveChangesAsync();
    }
}