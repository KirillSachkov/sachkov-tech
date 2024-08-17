using CSharpFunctionalExtensions;
using SachkovTech.Domain.Modules;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Modules;

public interface IModulesRepository
{
    Task<Guid> Add(Module module, CancellationToken cancellationToken = default);
    Task<Result<Module, Error>> GetById(ModuleId moduleId);
    Task<Result<Module, Error>> GetByTitle(Title title);
}