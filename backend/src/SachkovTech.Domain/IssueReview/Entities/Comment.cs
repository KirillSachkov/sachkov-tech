using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueReview.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueReview.Entities;

public class Comment : Shared.Entity<CommentId>
{
    //Ef core
    private Comment(CommentId id) : base(id)
    {
    }

    private Comment(CommentId id,
        CommentatorId commentatorId,
        Message message,
        DateTime createdAt) : base(id)
    {
        CommentatorId = commentatorId;
        Message = message;
        CreatedAt = createdAt;
    }

    public CommentatorId CommentatorId { get; private set; }

    public Message Message { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Result<Comment, Error> Create(CommentatorId commentatorId,
        Message message,
        DateTime createdAt)
    {
        return Result.Success<Comment, Error>(new Comment(
            CommentId.NewCommentId(),
            commentatorId,
            message,
            createdAt));
    }
}