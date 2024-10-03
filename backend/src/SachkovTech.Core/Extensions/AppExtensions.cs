
using Microsoft.AspNetCore.Builder;

namespace SachkovTech.Core.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        // await using var scope = app.Services.CreateAsyncScope();
        // var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        //
        // await dbContext.Database.MigrateAsync();
    }
}