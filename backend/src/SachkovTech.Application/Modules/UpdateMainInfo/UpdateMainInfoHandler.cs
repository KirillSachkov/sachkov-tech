using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;

namespace SachkovTech.Application.Modules.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IModulesRepository _modulesRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IModulesRepository modulesRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var moduleResult = await _modulesRepository.GetById(request.ModuleId, cancellationToken);
        if (moduleResult.IsFailure)
            return moduleResult.Error;

        var title = Title.Create(request.Dto.Title).Value;
        var description = Description.Create(request.Dto.Description).Value;

        moduleResult.Value.UpdateMainInfo(title, description);

        var result = await _modulesRepository.Save(moduleResult.Value, cancellationToken);

        _logger.LogInformation(
            "Updated module {title}, {description} with id {moduleId}",
            title,
            description,
            request.ModuleId);

        return result;
    }
}