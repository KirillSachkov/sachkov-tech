using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssueSolving.Application.Commands.SendOnReview;

public record SendOnReviewCommand(Guid UserIssueId, Guid UserId, string PullRequestUrl) : ICommand;