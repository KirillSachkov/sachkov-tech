using Microsoft.AspNetCore.Identity;

namespace SachkovTech.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; } = [];
}