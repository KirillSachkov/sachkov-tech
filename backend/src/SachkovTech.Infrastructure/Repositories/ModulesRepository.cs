using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Application.Modules;
using SachkovTech.Domain.IssueManagement;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Infrastructure.Repositories;

public class ModulesRepository : IModulesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ModulesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Module module, CancellationToken cancellationToken = default)
    {
        await _dbContext.Modules.AddAsync(module, cancellationToken);
        return module.Id;
    }

    public Guid Save(Module module, CancellationToken cancellationToken = default)
    {
        _dbContext.Modules.Attach(module);
        return module.Id.Value;
    }

    public Guid Delete(Module module, CancellationToken cancellationToken = default)
    {
        _dbContext.Modules.Remove(module);
        return module.Id;
    }

    public async Task<Result<Module, Error>> GetById(
        ModuleId moduleId, CancellationToken cancellationToken = default)
    {
        var module = await _dbContext.Modules
            .Include(m => m.Issues)
            .ThenInclude(i => i.SubIssues)
            .FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);

        if (module is null)
            return Errors.General.NotFound(moduleId);

        return module;
    }

    public async Task<Result<Module, Error>> GetByTitle(
        Title title, CancellationToken cancellationToken = default)
    {
        var module = await _dbContext.Modules
            .Include(m => m.Issues)
            .ThenInclude(i => i.SubIssues)
            .FirstOrDefaultAsync(m => m.Title == title, cancellationToken);

        if (module is null)
            return Errors.General.NotFound();

        return module;
    }
}