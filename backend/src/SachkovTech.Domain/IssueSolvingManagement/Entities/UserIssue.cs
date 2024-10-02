using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueSolvingManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueSolvingManagement.Entities;

public class UserIssue : Shared.Entity<UserIssueId>
{
    public UserIssue(
        UserIssueId id,
        Guid userId,
        IssueIdentifier issueIdentifier
        ) : base(id)
    {
        UserId = userId;
        IssueIdentifier = issueIdentifier;
        Status = IssueStatus.NotCompleted;
    }
    
    public Guid UserId { get; private set; }
    
    public IssueIdentifier IssueIdentifier { get; private set; }
    
    public IssueStatus Status { get; private set; }
    
    public DateTime StartDateOfExecution { get; private set; }
    
    public DateTime EndDateOfExecution { get; private set; }
    
    public Attempts Attempts { get; private set; }

    public PullRequestUrl PullRequestUrl { get; private set; }

    public void TakeTask()
    {
        StartDateOfExecution = DateTime.Now;
        Status = IssueStatus.AtWork;
        Attempts = Attempts.Create();
    }

    public UnitResult<Error> SendOnReview(PullRequestUrl pullRequestUrl)
    {
        EndDateOfExecution = DateTime.Now;

        if (Status != IssueStatus.AtWork)
            return Error.Failure("issue.status.invalid", "issue not at work");
        
        Status = IssueStatus.UnderReview;
        PullRequestUrl = pullRequestUrl;

        return Result.Success<Error>();
    }

    public void SendForRevision()
    {
        Status = IssueStatus.AtWork;
        Attempts = Attempts.Add();
    }

    public void CompleteTask()
    {
        Status = IssueStatus.Completed;
    }
}