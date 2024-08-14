using Microsoft.EntityFrameworkCore;
using SachkovTech.Domain.Modules;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Infrastructure.Repositories;

public class ModulesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ModulesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Module module, CancellationToken cancellationToken = default)
    {
        await _dbContext.Modules.AddAsync(module, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return module.Id;
    }

    public async Task<Result<Module>> GetById(ModuleId moduleId)
    {
        var module = await _dbContext.Modules
            .Include(m => m.Issues)
            .ThenInclude(i => i.SubIssues)
            .FirstOrDefaultAsync(m => m.Id == moduleId);

        if (module is null)
            return "Module not found";

        return module;
    }
}