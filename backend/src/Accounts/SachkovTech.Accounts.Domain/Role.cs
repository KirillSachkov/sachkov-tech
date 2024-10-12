using Microsoft.AspNetCore.Identity;

namespace SachkovTech.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public virtual List<IdentityUserRole<Guid>> UserRoles { get; set; } = [];
    public virtual List<RolePermission> RolePermissions { get; set; } = [];
}