using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared.ValueObjects;

public class Title: ValueObject
{
    public const int MAX_LENGTH = 100;

    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
            return Errors.General.ValueIsInvalid("Title");

        return new Title(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}