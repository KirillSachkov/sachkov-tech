using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects.Ids;

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
    
    public static implicit operator UserIssueId(Guid id) => new(id);

    public static implicit operator Guid(UserIssueId userIssueId)
    {
        ArgumentNullException.ThrowIfNull(userIssueId);
        return userIssueId.Value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}