using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Commands.RestoreIssue;

public class RestoreIssueHandler : ICommandHandler<Guid, RestoreIssueCommand>
{
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RestoreIssueHandler> _logger;

    public RestoreIssueHandler(
        IModulesRepository modulesRepository,
        [FromKeyedServices(Modules.Issues)] IUnitOfWork unitOfWork,
        ILogger<RestoreIssueHandler> logger)
    {
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RestoreIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var moduleResult = await _modulesRepository.GetById(command.ModuleId, cancellationToken);

        if (moduleResult.IsFailure)
            return moduleResult.Error.ToErrorList();

        var restoreResult = moduleResult.Value.RestoreIssue(command.IssueId);

        if (restoreResult.IsFailure)
            return restoreResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Issue {issueId} was restored in module {moduleId}",
            command.IssueId,
            command.ModuleId);

        return command.IssueId;
    }
}