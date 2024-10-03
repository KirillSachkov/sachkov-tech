using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public class UserIssueId : ValueObject
{
    private UserIssueId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static UserIssueId NewIssueId() => new(Guid.NewGuid());
    public static UserIssueId Empty() => new(Guid.Empty);
    public static UserIssueId Create(Guid id) => new(id);
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}