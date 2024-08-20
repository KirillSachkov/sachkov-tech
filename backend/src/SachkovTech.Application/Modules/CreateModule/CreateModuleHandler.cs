using CSharpFunctionalExtensions;
using FluentValidation;
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
        var title = Title.Create(request.Title).Value;
        var description = Description.Create(request.Description).Value;

        var module = await _modulesRepository.GetByTitle(title);

        if (module.IsSuccess)
            return Errors.Module.AlreadyExist();

        var moduleId = ModuleId.NewModuleId();

        var moduleToCreate = new Module(moduleId, title, description);

        await _modulesRepository.Add(moduleToCreate, cancellationToken);

        return (Guid)moduleToCreate.Id;
    }
}