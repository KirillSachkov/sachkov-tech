using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Contracts;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Domain;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Commands.Create;

public class CreateModuleHandler : ICommandHandler<Guid, CreateModuleCommand>
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateModuleCommand> _validator;
    private readonly ILogger<CreateModuleHandler> _logger;
    private readonly IAccountsContract _accountsContract;

    public CreateModuleHandler(
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateModuleCommand> validator,
        IAccountsContract accountsContract,
        ILogger<CreateModuleHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _accountsContract = accountsContract;
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
            return Errors.General.AlreadyExist().ToErrorList();

        var moduleId = ModuleId.NewModuleId();

        var moduleToCreate = new Module(moduleId, title, description);

        await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        await _modulesRepository.Add(moduleToCreate, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        // do some work

        await _accountsContract.SaveChanges(transaction, cancellationToken);

        _logger.LogInformation("Created module {title} with id {moduleId}", title, moduleId);

        await transaction.CommitAsync(cancellationToken);

        return (Guid)moduleToCreate.Id;
    }
}