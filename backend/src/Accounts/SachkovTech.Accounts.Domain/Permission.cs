namespace SachkovTech.Accounts.Domain;

public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;

    public virtual List<RolePermission> RolePermissions { get; set; } = [];
}