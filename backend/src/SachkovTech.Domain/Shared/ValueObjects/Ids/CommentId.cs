namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public record CommentId
{
    private CommentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static CommentId NewCommentId() => new(Guid.NewGuid());
    public static CommentId Empty() => new(Guid.Empty);
    public static CommentId Create(Guid id) => new(id);
}