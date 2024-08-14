using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public record Description
{
    public const int MAX_LENGTH = 2000;

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
            return "Description is invalid";

        return new Description(value);
    }
}