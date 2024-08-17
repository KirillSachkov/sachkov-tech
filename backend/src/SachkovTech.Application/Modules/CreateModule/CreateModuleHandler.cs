using CSharpFunctionalExtensions;
using SachkovTech.Domain.Modules;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Modules.CreateModule;

public class CreateModuleHandler
{
    private readonly IModulesRepository _modulesRepository;

    public CreateModuleHandler(IModulesRepository modulesRepository)
    {
        _modulesRepository = modulesRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateModuleRequest request, CancellationToken cancellationToken = default)
    {
        var titleResult = Title.Create(request.Title);
        if (titleResult.IsFailure)
            return titleResult.Error;

        var descriptionResult = Description.Create(request.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;

        var module = await _modulesRepository.GetByTitle(titleResult.Value);

        if (module.IsSuccess)
            return Errors.Module.AlreadyExist();

        var moduleId = ModuleId.NewModuleId();

        var moduleToCreate = new Module(moduleId, titleResult.Value, descriptionResult.Value);

        await _modulesRepository.Add(moduleToCreate, cancellationToken);

        return (Guid)moduleToCreate.Id;
    }
}