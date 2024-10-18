using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Issues.Application;
using SachkovTech.Issues.Domain;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Repositories;

public class ModulesRepository : IModulesRepository
{
    private readonly IssuesWriteDbContext _dbContext;

    public ModulesRepository(IssuesWriteDbContext dbContext)
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

    public Guid Delete(Module module)
    {
        _dbContext.Modules.Remove(module);

        return module.Id;
    }

    public async Task<Result<Module, Error>> GetById(
        ModuleId moduleId, CancellationToken cancellationToken = default)
    {
        var module = await _dbContext.Modules
            .Include(m => m.Issues)
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
            .FirstOrDefaultAsync(m => m.Title == title, cancellationToken);

        if (module is null)
            return Errors.General.NotFound();

        return module;
    }
}