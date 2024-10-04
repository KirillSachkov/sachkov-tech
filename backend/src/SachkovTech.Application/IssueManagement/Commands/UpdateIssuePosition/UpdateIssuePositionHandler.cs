using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Abstraction;
using SachkovTech.Application.Database;
using SachkovTech.Application.Extensions;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.IssueManagement.Commands.UpdatePosition;

public class UpdateIssuePositionHandler : ICommandHandler<Guid, UpdateIssuePositionCommand>
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateIssuePositionCommand> _validator;
    private readonly ILogger<UpdateIssuePositionHandler> _logger;

    public UpdateIssuePositionHandler(
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateIssuePositionCommand> validator,
        ILogger<UpdateIssuePositionHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateIssuePositionCommand command,
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
        
        var newPosition = Position.Create(command.NewPosition).Value;
        moduleResult.Value.MoveIssue(issueResult, newPosition);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Changed issue position with id {issueId} in module {moduleId}",
            command.IssueId,
            command.ModuleId);

        return moduleResult.Value.Id.Value;
    }
}