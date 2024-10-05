using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

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

    public static Result<FileType, Error> Parse(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName).ToLower();

        string type = "";

        switch (fileExtension)
        {
            case ".png":
                type = Image.Value;
                break;
            case ".jpeg":
                type = Image.Value;
                break;
            case ".gif":
                type = Image.Value;
                break;
            case ".svg":
                type = Image.Value;
                break;
            case ".icon":
                type = Image.Value;
                break;
        }

        return Create(type);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}