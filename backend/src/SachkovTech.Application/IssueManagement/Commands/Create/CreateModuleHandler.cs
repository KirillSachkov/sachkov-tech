using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Abstraction;
using SachkovTech.Application.Database;
using SachkovTech.Application.Extensions;
using SachkovTech.Domain.IssueManagement;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Application.IssueManagement.Commands.Create;

public class CreateModuleHandler : ICommandHandler<Guid, CreateModuleCommand>
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateModuleCommand> _validator;
    private readonly ILogger<CreateModuleHandler> _logger;

    public CreateModuleHandler(
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateModuleCommand> validator,
        ILogger<CreateModuleHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateModuleCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }
        
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;

        var module = await _modulesRepository.GetByTitle(title, cancellationToken);

        if (module.IsSuccess)
            return Errors.Module.AlreadyExist().ToErrorList();

        var moduleId = ModuleId.NewModuleId();

        var moduleToCreate = new Module(moduleId, title, description);

        await _modulesRepository.Add(moduleToCreate, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created module {title} with id {moduleId}", title, moduleId);

        return (Guid)moduleToCreate.Id;
    }
}