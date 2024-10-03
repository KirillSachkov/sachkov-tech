using CSharpFunctionalExtensions;
using SachkovTech.Core;
using SachkovTech.Core.ValueObjects.Ids;
using SachkovTech.Domain.IssueReview.ValueObjects;

namespace SachkovTech.Domain.IssueReview.Entities;

public class Comment : Entity<CommentId>
{
    //Ef core
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