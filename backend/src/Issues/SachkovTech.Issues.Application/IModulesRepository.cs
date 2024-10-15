using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application;

public interface IModulesRepository
{
    Task<Guid> Add(Module module, CancellationToken cancellationToken = default);
    Guid Save(Module module, CancellationToken cancellationToken = default);
    Guid Delete(Module module);
    Task<Result<Module, Error>> GetById(ModuleId moduleId, CancellationToken cancellationToken = default);
    Task<Result<Module, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default);
}