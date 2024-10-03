using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using SachkovTech.Core;

namespace SachkovTech.Domain.IssueSolvingManagement.ValueObjects;

public class PullRequestUrl : ValueObject
{
    private const string PATTERN = @"^https:\/\/github\.com\/[^\/]+\/[^\/]+\/pull\/\d+$";
    
    public PullRequestUrl(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<PullRequestUrl, Error> Create(string pullRequestUrl)
    {
        if (!Regex.IsMatch(pullRequestUrl, PATTERN))
            return Errors.General.ValueIsInvalid("pull request url");
        
        return new PullRequestUrl(pullRequestUrl);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}