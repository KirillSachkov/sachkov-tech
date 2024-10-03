using CSharpFunctionalExtensions;
using SachkovTech.Core;
using SachkovTech.Core.ValueObjects;
using SachkovTech.Core.ValueObjects.Ids;
using SachkovTech.Issues.Domain;

namespace SachkovTech.Issues.Application.IssueManagement;

public interface IModulesRepository
{
    Task<Guid> Add(Module module, CancellationToken cancellationToken = default);
    Guid Save(Module module, CancellationToken cancellationToken = default);
    Guid Delete(Module module, CancellationToken cancellationToken = default);
    Task<Result<Module, Error>> GetById(ModuleId moduleId, CancellationToken cancellationToken = default);
    Task<Result<Module, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default);
}