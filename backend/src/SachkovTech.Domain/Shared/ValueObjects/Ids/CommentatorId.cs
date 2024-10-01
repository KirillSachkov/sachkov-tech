namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public record CommentatorId
{
    private CommentatorId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static CommentatorId NewCommentatorId() => new(Guid.NewGuid());
    public static CommentatorId Empty() => new(Guid.Empty);
    public static CommentatorId Create(Guid id) => new(id);
}