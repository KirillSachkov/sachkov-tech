using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Modules.Delete;

public class DeleteModuleHandler
{
    private readonly IModulesRepository _modulesRepository;
    private readonly ILogger<DeleteModuleHandler> _logger;

    public DeleteModuleHandler(
        IModulesRepository modulesRepository,
        ILogger<DeleteModuleHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteModuleRequest request,
        CancellationToken cancellationToken = default)
    {
        var moduleResult = await _modulesRepository.GetById(request.ModuleId, cancellationToken);
        if (moduleResult.IsFailure)
            return moduleResult.Error;

        var result = await _modulesRepository.Delete(moduleResult.Value, cancellationToken);

        _logger.LogInformation("Updated deleted with id {moduleId}", request.ModuleId);

        return result;
    }
}