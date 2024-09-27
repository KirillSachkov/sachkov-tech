using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Authorization.DataModels;

namespace SachkovTech.Infrastructure.Authorization;

public class AuthorizationSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthorizationSeeder(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SeedAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        var json = await File.ReadAllTextAsync("etc/permissions.json");
        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)!;

        await SeedPermissions(seedData, context);

        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await GetOrCreateRole(roleManager, roleName);

            foreach (var permissionCode in seedData.Roles[roleName])
            {
                await SeedRolePermissions(context, permissionCode, role);
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolePermissions(AuthorizationDbContext context, string permissionCode, Role role)
    {
        var permission = await context.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
        if (permission != null &&
            !context.RolePermissions.Any(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id))
        {
            context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permission.Id });
        }
    }

    private static async Task<Role> GetOrCreateRole(RoleManager<Role> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is not null) return role;
        role = new Role { Name = roleName };
        await roleManager.CreateAsync(role);

        return role;
    }

    private static async Task SeedPermissions(RolePermissionConfig seedData, AuthorizationDbContext context)
    {
        var permissionsToAdd = seedData.Permissions.SelectMany(permissionGroup => permissionGroup.Value);
        foreach (var permissionCode in permissionsToAdd)
        {
            if (!await context.Permissions.AnyAsync(p => p.Code == permissionCode))
            {
                context.Permissions.Add(new Permission { Code = permissionCode });
            }
        }

        await context.SaveChangesAsync();
    }
}