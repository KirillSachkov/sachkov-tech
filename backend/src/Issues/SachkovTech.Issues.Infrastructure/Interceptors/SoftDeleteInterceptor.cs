using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Infrastructure.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted 
                        && (e.Entity.DeletionDate is null 
                            || e.Entity.DeletionDate! >= DateTime.UtcNow
                                .AddDays(Constants.LIFETIME_AFTER_DELETION)));

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.Delete();
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}