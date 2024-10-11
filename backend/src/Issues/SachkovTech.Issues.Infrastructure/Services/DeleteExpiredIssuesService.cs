using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Domain;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.Infrastructure.Services;

public class DeleteExpiredIssuesService
{
    private readonly WriteDbContext _writeDbContext;

    public DeleteExpiredIssuesService(
        WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    
    public async Task Process(CancellationToken cancellationToken)
    {
        var modules = await GetModulesWithIssuesAsync(cancellationToken);

        foreach (var module in modules)
        {
            DeleteExpiredIssuesFromModule(module);
        }

        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Module>> GetModulesWithIssuesAsync(CancellationToken cancellationToken)
    {
        return await _writeDbContext.Modules.Include(m => m.Issues).ToListAsync(cancellationToken);
    }

    private void DeleteExpiredIssuesFromModule(Module module)
    {
        var expiredTasks = module.Issues
            .Where(i => i.DeletionDate != null 
                        && i.DeletionDate < DateTime.UtcNow.AddDays(Constants.LIFETIME_AFTER_DELETION))
            .ToList();

        foreach (var expiredTask in expiredTasks)
        {
            module.DeleteIssue(expiredTask.Id);
        }
    }
}