using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager(AccountsDbContext accountsContext)
{
    public async Task<Permission?> FindByCode(string code)
        => await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);

    public async Task AddRangeIfExist(
        IEnumerable<string> permissions, CancellationToken cancellationToken = default)
    {
        foreach (var permissionCode in permissions)
        {
            var isPermissionExist = await accountsContext.Permissions
                .AnyAsync(p => p.Code == permissionCode, cancellationToken);

            if (isPermissionExist)
                return;

            await accountsContext.Permissions.AddAsync(new Permission { Code = permissionCode }, cancellationToken);
        }

        await accountsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HashSet<string>> GetUserPermissionCodes(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var permissions = await accountsContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync(cancellationToken);

        return permissions.ToHashSet();
    }
}