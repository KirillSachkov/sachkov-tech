using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Shared;

public record FullName
{
    private FullName(string firstName, string secondName)
    {
        FirstName = firstName;
        SecondName = secondName;
    }

    public string FirstName { get; }
    public string SecondName { get; }

    public static Result<FullName, Error> Create(string firstName, string secondName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsInvalid("Title");

        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsInvalid("Title");


        return new FullName(firstName, secondName);
    }
}