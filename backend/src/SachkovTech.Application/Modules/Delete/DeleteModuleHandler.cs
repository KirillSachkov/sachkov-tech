using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Database;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Modules.Delete;

public class DeleteModuleHandler
{
    private readonly IModulesRepository _modulesRepository;
    private readonly ILogger<DeleteModuleHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteModuleHandler(
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteModuleHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteModuleRequest request,
        CancellationToken cancellationToken = default)
    {
        var moduleResult = await _modulesRepository.GetById(request.ModuleId, cancellationToken);
        if (moduleResult.IsFailure)
            return moduleResult.Error;
        
        moduleResult.Value.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Updated deleted with id {moduleId}", request.ModuleId);

        return moduleResult.Value.Id.Value;
    }
}