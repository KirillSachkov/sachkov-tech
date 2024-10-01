using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.IssueReview.ValueObjects;

public record PullRequestLink
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
}