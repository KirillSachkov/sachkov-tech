using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Domain.IssueManagement;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Application.Modules.Create;

public class CreateModuleHandler
{
    private readonly IModulesRepository _modulesRepository;
    private readonly ILogger<CreateModuleHandler> _logger;

    public CreateModuleHandler(
        IModulesRepository modulesRepository,
        ILogger<CreateModuleHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateModuleRequest request, CancellationToken cancellationToken = default)
    {
        var title = Title.Create(request.Title).Value;
        var description = Description.Create(request.Description).Value;

        var module = await _modulesRepository.GetByTitle(title, cancellationToken);

        if (module.IsSuccess)
            return Errors.Module.AlreadyExist();

        var moduleId = ModuleId.NewModuleId();

        var moduleToCreate = new Module(moduleId, title, description);

        await _modulesRepository.Add(moduleToCreate, cancellationToken);

        _logger.LogInformation("Created module {title} with id {moduleId}", title, moduleId);

        return (Guid)moduleToCreate.Id;
    }
}