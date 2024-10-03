using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared.ValueObjects.Ids;

public class IssueReviewId : ValueObject
{
    private IssueReviewId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static IssueReviewId NewIssueReviewId() => new(Guid.NewGuid());
    public static IssueReviewId Empty() => new(Guid.Empty);
    public static IssueReviewId Create(Guid id) => new(id);
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}