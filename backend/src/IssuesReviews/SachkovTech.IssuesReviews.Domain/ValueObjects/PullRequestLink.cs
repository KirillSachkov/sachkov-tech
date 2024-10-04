using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Domain.ValueObjects;

public class PullRequestLink : ValueObject
{
    private PullRequestLink(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PullRequestLink, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.General.ValueIsInvalid(nameof(Message));
        }

        if (value.StartsWith("https://github.com/") == false)
        {
            return Errors.General.ValueIsInvalid(nameof(Message));
        }

        return new PullRequestLink(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}