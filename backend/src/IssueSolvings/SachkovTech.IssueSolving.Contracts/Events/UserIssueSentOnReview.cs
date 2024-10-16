using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssueSolving.Contracts.Events;

public class UserIssueSentOnReview : IDomainEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public Guid UserIssueId { get; init; }
    public Guid UserId { get; init; }
    public string PullRequestUrl { get; init; } = string.Empty;
}