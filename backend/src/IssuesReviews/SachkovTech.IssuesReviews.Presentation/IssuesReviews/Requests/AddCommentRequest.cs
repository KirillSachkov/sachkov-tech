using SachkovTech.IssuesReviews.Application.Commands.AddComment;

namespace SachkovTech.IssuesReviews.Presentation.IssuesReviews.Requests;

public record AddCommentRequest(
    string Message)
{
    public AddCommentCommand ToCommand(Guid issueReviewId, Guid userId) =>
        new (issueReviewId, userId, Message);
}