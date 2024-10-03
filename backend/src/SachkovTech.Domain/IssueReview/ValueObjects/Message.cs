using CSharpFunctionalExtensions;
using SachkovTech.Core;

namespace SachkovTech.Domain.IssueReview.ValueObjects;

public class Message : ValueObject 
{
    private Message(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Message, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.General.ValueIsInvalid(nameof(Message));
        }

        if (value.Length > Constants.Default.MAX_HIGH_TEXT_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(Message));
        }
        
        return new Message(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}