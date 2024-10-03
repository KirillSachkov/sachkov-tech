using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class FileAttribute : ValueObject
{
    public string Value { get; }

    private FileAttribute(string value)
    {
        Value = value;
    }

    public static Result<FileAttribute, Error> Create(string fileAttribute)
    {
        if (string.IsNullOrEmpty(fileAttribute))
            return Errors.General.ValueIsInvalid("file attribute");

        return new FileAttribute(fileAttribute);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(FileAttribute fileAttribute) =>
        fileAttribute.Value;
}