using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public record Title
{
    public const int MAX_LENGTH = 100;

    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
            return "Title is invalid";

        return new Title(value);
    }
}