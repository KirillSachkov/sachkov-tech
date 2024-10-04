using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

public class FilePath : ValueObject
{
    public string Value { get; }

    private FilePath(string value)
    {
        Value = value;
    }

    public static Result<FilePath, Error> Create(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(filePath);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(FilePath filePath) =>
        filePath.Value;
}