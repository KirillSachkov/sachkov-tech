using CSharpFunctionalExtensions;
using SachkovTech.IssueSolving.Domain.Enums;
using SachkovTech.IssueSolving.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Domain.Entities;

public class UserIssue : Entity<UserIssueId>
{
    //ef core
    private UserIssue(UserIssueId id) : base(id)
    {
        
    }
    public UserIssue(
        UserIssueId id,
        UserId userId,
        IssueId issueId) : base(id)
    {
        UserId = userId;
        IssueId = issueId;
        Status = IssueStatus.NotCompleted;
        
        TakeOnWork();
    }

    public UserId UserId { get; private set; }

    public IssueId IssueId { get; private set; }

    public IssueStatus Status { get; private set; }

    public DateTime StartDateOfExecution { get; private set; }

    public DateTime EndDateOfExecution { get; private set; }

    public Attempts Attempts { get; private set; } = null!;

    public PullRequestUrl PullRequestUrl { get; private set; } = null!;

    private void TakeOnWork()
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

    public UnitResult<Error> SendForRevision()
    {
        if (Status == IssueStatus.NotCompleted || Status == IssueStatus.UnderReview)
        {
            Status = IssueStatus.AtWork;
            Attempts = Attempts.Add();

            return Result.Success<Error>();
        }

        return Error.Failure("issue.status.invalid", "issue status should be not completed or under review");
    }

    public UnitResult<Error> CompleteTask()
    {
        if (Status != IssueStatus.UnderReview)
            return Error.Failure("issue.invalid.status", "issue status should be under review");

        Status = IssueStatus.Completed;

        return new UnitResult<Error>();
    }
}