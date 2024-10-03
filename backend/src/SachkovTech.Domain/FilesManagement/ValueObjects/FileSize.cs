using CSharpFunctionalExtensions;
using SachkovTech.Core;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class FileSize : ValueObject
{
    public long Value { get; }
    
    private FileSize(long value)
    {
        Value = value;
    }

    public static Result<FileSize, Error> Create(long fileLength)
    {
        if (fileLength <= 0)
            return Errors.General.ValueIsInvalid("file length");

        return new FileSize(fileLength);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator long(FileSize fileSize) =>
        fileSize.Value;
}