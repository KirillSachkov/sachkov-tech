namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public record ReviewerId
{
    private ReviewerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static ReviewerId NewReviewerId() => new(Guid.NewGuid());
    public static ReviewerId Empty() => new(Guid.Empty);
    public static ReviewerId Create(Guid id) => new(id);
}