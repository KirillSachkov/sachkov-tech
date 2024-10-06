using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Contracts;
using SachkovTech.IssueSolving.Domain.Entities;
using SachkovTech.IssueSolving.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Application.Commands.TakeOnWork;

public class TakeOnWorkHandler : ICommandHandler<Guid, TakeOnWorkCommand>
{
    private readonly IUserIssueRepository _repository;
    private readonly IReadDbContext _readDbContext;
    private readonly IIssuesContract _issuesContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TakeOnWorkHandler> _logger;

    public TakeOnWorkHandler(
        IUserIssueRepository repository,
        IReadDbContext readDbContext,
        IIssuesContract issuesContract,
        IUnitOfWork unitOfWork,
        ILogger<TakeOnWorkHandler> logger)
    {
        _repository = repository;
        _readDbContext = readDbContext;
        _issuesContract = issuesContract;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        TakeOnWorkCommand command,
        CancellationToken cancellationToken = default)
    {
        var issueResult = await _issuesContract.GetIssueById(command.IssueId, cancellationToken);

        if (issueResult.IsFailure)
            return issueResult.Error;

        if (issueResult.Value.Position > 1)
        {
            var previousIssueResult = await _issuesContract
                .GetIssueByPosition(issueResult.Value.Position - 1, cancellationToken);

            if (previousIssueResult.IsFailure)
                return previousIssueResult.Error;
        
            var previousUserIssue = await _readDbContext.UserIssues
                .FirstOrDefaultAsync(u => u.UserId == command.UserId && 
                                          u.IssueId == previousIssueResult.Value.Id, cancellationToken);

            if (previousUserIssue is null)
                return Errors.General.NotFound().ToErrorList();

            var previousUserIssueStatus = Enum.Parse<IssueStatus>(previousUserIssue.Status);

            if (previousUserIssueStatus != IssueStatus.Completed)
                return Error.Failure("prev.issue.not.solved", "previous issue not solved").ToErrorList();
        }

        var userIssueId = UserIssueId.NewIssueId();
        var userId = UserId.Create(command.UserId);

        var userIssue = new UserIssue(userIssueId, userId, command.IssueId);
        
        userIssue.TakeOnWork();

        var result = await _repository.Add(userIssue, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("User took issue on work. A record was created with id {userIssueId}",
            userIssueId);

        return result;
    }
}