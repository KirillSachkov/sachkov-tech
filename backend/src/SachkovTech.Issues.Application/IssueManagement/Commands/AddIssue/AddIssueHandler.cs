using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Core;
using SachkovTech.Core.Abstraction;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.ValueObjects;
using SachkovTech.Core.ValueObjects.Ids;
using SachkovTech.Issues.Domain.Entities;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.AddIssue;

public class AddIssueHandler : ICommandHandler<Guid, AddIssueCommand>
{
    private const string BUCKET_NAME = "files";

    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddIssueCommand> _validator;
    private readonly ILogger<AddIssueHandler> _logger;

    public AddIssueHandler(
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddIssueCommand> validator,
        ILogger<AddIssueHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var moduleResult = await _modulesRepository
            .GetById(ModuleId.Create(command.ModuleId), cancellationToken);

        if (moduleResult.IsFailure)
            return moduleResult.Error.ToErrorList();

        var issue = InitIssue(command);
        moduleResult.Value.AddIssue(issue);

        await _unitOfWork.SaveChanges(cancellationToken);

        return issue.Id.Value;
    }

    private Issue InitIssue(AddIssueCommand command)
    {
        var issueId = IssueId.NewIssueId();
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var lessonId = LessonId.Empty();

        return new Issue(
            issueId,
            title,
            description,
            lessonId,
            null);
    }
}