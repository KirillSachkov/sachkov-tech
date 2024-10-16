using Microsoft.AspNetCore.Identity;

namespace SachkovTech.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public static readonly string Admin = nameof(Admin);
    public static readonly string Participant = nameof(Participant);
    public List<RolePermission> RolePermissions { get; set; } = [];
}