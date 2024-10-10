using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure;

public class RolePermissionManager(AccountsDbContext accountsContext)
{
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);

            var rolePermissionExist = await accountsContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);

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