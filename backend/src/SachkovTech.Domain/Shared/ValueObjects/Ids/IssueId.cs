using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public class IssueId : ValueObject
{
    private IssueId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static IssueId NewIssueId() => new(Guid.NewGuid());
    public static IssueId Empty() => new(Guid.Empty);
    public static IssueId Create(Guid id) => new(id);
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

}