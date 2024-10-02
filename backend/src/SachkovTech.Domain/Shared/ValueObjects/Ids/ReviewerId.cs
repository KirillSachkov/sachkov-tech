using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public class ReviewerId : ValueObject
{
    private ReviewerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static ReviewerId NewReviewerId() => new(Guid.NewGuid());
    public static ReviewerId Empty() => new(Guid.Empty);
    public static ReviewerId Create(Guid id) => new(id);
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}