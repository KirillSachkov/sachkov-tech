using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Domain.Entities;

public class Comment : Entity<CommentId>
{
    //Ef core
    public IssueReview IssueReview { get; private set; }
    private Comment(CommentId id) : base(id)
    {
    }

    private Comment(CommentId id,
        UserId userId,
        Message message,
        DateTime createdAt) : base(id)
    {
        UserId = userId;
        Message = message;
        CreatedAt = createdAt;
    }

    public UserId UserId { get; private set; }

    public Message Message { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Result<Comment, Error> Create(UserId userId,
        Message message)
    {
        return Result.Success<Comment, Error>(new Comment(
            CommentId.NewCommentId(),
            userId,
            message,
            DateTime.UtcNow));
    }
}