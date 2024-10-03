using CSharpFunctionalExtensions;

namespace SachkovTech.Core.ValueObjects.Ids;

public class UserId : ValueObject
{
    private UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static UserId NewUserId() => new(Guid.NewGuid());
    public static UserId Empty() => new(Guid.Empty);
    public static UserId Create(Guid id) => new(id);
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}