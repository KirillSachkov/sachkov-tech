using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared.ValueObjects;

public class Description : ValueObject
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

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}