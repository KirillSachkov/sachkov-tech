using Microsoft.EntityFrameworkCore;
using SachkovTech.Infrastructure;
using SachkovTech.Infrastructure.DbContexts;

namespace SachkovTech.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}