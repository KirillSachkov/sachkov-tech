using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueManagement;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Application.IssueManagement;

public interface IModulesRepository
{
    Task<Guid> Add(Module module, CancellationToken cancellationToken = default);
    Guid Save(Module module, CancellationToken cancellationToken = default);
    Guid Delete(Module module, CancellationToken cancellationToken = default);
    Task<Result<Module, Error>> GetById(ModuleId moduleId, CancellationToken cancellationToken = default);
    Task<Result<Module, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default);
}