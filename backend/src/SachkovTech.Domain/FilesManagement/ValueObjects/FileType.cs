using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class FileType : ValueObject
{
    public static readonly FileType Image = new(nameof(Image).ToUpper());
    public static readonly FileType Video = new(nameof(Video).ToUpper());
    public static readonly FileType Document = new(nameof(Document).ToUpper());
    public static readonly FileType Spreadsheets = new(nameof(Spreadsheets).ToUpper());
    public static readonly FileType Archive = new(nameof(Archive).ToUpper());
    
    public string Value { get; }

    private FileType(string value)
    {
        Value = value;
    }

    public static Result<FileType, Error> Create(string fileType)
    {
        if (string.IsNullOrWhiteSpace(fileType) || fileType.Length > Constants.Default.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("file type");

        return new FileType(fileType);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}