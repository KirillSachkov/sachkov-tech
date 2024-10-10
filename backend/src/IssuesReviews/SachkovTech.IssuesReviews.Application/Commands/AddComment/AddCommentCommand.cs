using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Commands.AddComment;

public record AddCommentCommand(
    Guid IssueReviewId,
    Guid UserId,
    string Message) : ICommand;