using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AccountsSeeder> _logger;

    public AccountsSeeder(IServiceScopeFactory serviceScopeFactory, ILogger<AccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding accounts...");

        var json = await File.ReadAllTextAsync("etc/accounts.json");

        using var scope = _serviceScopeFactory.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        var rolePermissionManager = scope.ServiceProvider.GetRequiredService<RolePermissionManager>();

        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission config.");

        await SeedPermissions(seedData, permissionManager);

        await SeedRoles(seedData, roleManager);

        await SeedRolePermissions(seedData, roleManager, rolePermissionManager);
    }

    private async Task SeedRolePermissions(RolePermissionConfig seedData, RoleManager<Role> roleManager,
        RolePermissionManager rolePermissionManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var rolePermissions = seedData.Roles[roleName];

            await rolePermissionManager.AddRangeIfExist(role!.Id, rolePermissions);
        }

        _logger.LogInformation("Role permissions added to database.");
    }

    private async Task SeedRoles(RolePermissionConfig seedData, RoleManager<Role> roleManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        _logger.LogInformation("Roles added to database.");
    }

    private async Task SeedPermissions(RolePermissionConfig seedData, PermissionManager permissionManager)
    {
        var permissionsToAdd = seedData.Permissions.SelectMany(permissionGroup => permissionGroup.Value);

        await permissionManager.AddRangeIfExist(permissionsToAdd);

        _logger.LogInformation("Permissions added to database.");
    }
}