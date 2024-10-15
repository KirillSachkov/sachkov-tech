using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Commands.Create;

public record CreateIssueReviewCommand(Guid UserIssueId, Guid UserId, string PullRequestUrl) : ICommand;