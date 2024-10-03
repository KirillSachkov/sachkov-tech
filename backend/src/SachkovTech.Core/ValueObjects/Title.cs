using CSharpFunctionalExtensions;

namespace SachkovTech.Core.ValueObjects;

public record Title
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
}