using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Domain;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.Infrastructure.Services;

public class DeleteExpiredIssuesService
{
    private readonly IsssuesWriteDbContext _isssuesWriteDbContext;

    public DeleteExpiredIssuesService(
        IsssuesWriteDbContext isssuesWriteDbContext)
    {
        _isssuesWriteDbContext = isssuesWriteDbContext;
    }
    
    public async Task Process(CancellationToken cancellationToken)
    {
        var modules = await GetModulesWithIssuesAsync(cancellationToken);

        foreach (var module in modules)
        {
            module.DeleteExpiredIssues();
        }

        await _isssuesWriteDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Module>> GetModulesWithIssuesAsync(CancellationToken cancellationToken)
    {
        return await _isssuesWriteDbContext.Modules.Include(m => m.Issues).ToListAsync(cancellationToken);
    }
}