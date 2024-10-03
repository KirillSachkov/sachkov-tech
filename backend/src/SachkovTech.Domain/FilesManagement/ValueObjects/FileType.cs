using CSharpFunctionalExtensions;
using SachkovTech.Core;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class FileType : ValueObject
{
    public static readonly FileType Image = new(nameof(Image).ToUpper());
    public static readonly FileType Video = new(nameof(Video).ToUpper());
    public static readonly FileType Document = new(nameof(Document).ToUpper());
    public static readonly FileType Spreadsheets = new(nameof(Spreadsheets).ToUpper());
    public static readonly FileType Archive = new(nameof(Archive).ToUpper());

    private static readonly FileType[] _types = [Image, Video, Document, Spreadsheets, Archive];

    public string Value { get; }

    private FileType(string value)
    {
        Value = value;
    }

    public static Result<FileType, Error> Create(string input)
    {
        var fileType = input.Trim().ToUpper();

        if (_types.Any(t => t.Value == fileType) == false)
        {
            return Errors.General.ValueIsInvalid("file type");
        }

        return new FileType(fileType);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}