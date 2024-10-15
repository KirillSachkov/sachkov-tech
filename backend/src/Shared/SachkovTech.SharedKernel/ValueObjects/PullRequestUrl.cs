using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects;

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

    public static readonly PullRequestUrl Empty = new PullRequestUrl("");
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}