using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Abstraction;
using SachkovTech.Application.Database;
using SachkovTech.Application.Extensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.IssueManagement.Commands.DeleteIssue;
public class SoftDeleteIssueHandler : ICommandHandler<Guid, DeleteIssueCommand>
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteIssueCommand> _validator;
    private readonly ILogger<SoftDeleteIssueHandler> _logger;

    public SoftDeleteIssueHandler(
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteIssueCommand> validator,
        ILogger<SoftDeleteIssueHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var moduleResult = await _modulesRepository.GetById(command.ModuleId, cancellationToken);
        if (moduleResult.IsFailure)
            return moduleResult.Error.ToErrorList();

        var issueResult = moduleResult.Value.Issues.FirstOrDefault(i => i.Id.Value == command.IssueId);
        if (issueResult == null)
            return Errors.General.NotFound(command.IssueId).ToErrorList();
        
        issueResult.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Issue {issueId} was SOFT deleted in module {moduleId}",
            command.IssueId,
            command.ModuleId);

        return moduleResult.Value.Id.Value;
    }
}