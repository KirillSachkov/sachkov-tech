using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects;

public record Description
{
    public const int MAX_LENGTH = 2000;

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
            return Errors.General.ValueIsInvalid("Description");

        return new Description(value);
    }
}