using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects.Ids;

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

    public static implicit operator IssueId(Guid id) => new(id);

    public static implicit operator Guid(IssueId issueId)
    {
        ArgumentNullException.ThrowIfNull(issueId);
        return issueId.Value;
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

}