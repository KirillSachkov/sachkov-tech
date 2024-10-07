using System.Net.Mime;
using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

public class MimeType : ValueObject
{
    public static readonly MimeType Png = new(MediaTypeNames.Image.Png);
    public static readonly MimeType Jpeg = new(MediaTypeNames.Image.Jpeg);
    public static readonly MimeType Gif = new(MediaTypeNames.Image.Gif);
    public static readonly MimeType Svg = new(MediaTypeNames.Image.Svg);
    public static readonly MimeType Icon = new(MediaTypeNames.Image.Icon);

    public static readonly MimeType Pdf = new(MediaTypeNames.Application.Pdf);
    public static readonly MimeType Json = new(MediaTypeNames.Application.Json);
    public static readonly MimeType Xml = new(MediaTypeNames.Application.Xml);

    private static readonly MimeType[] _types =
    [
        Png, Jpeg, Gif, Svg, Icon, Pdf, Json, Xml, Svg
    ];

    public string Value { get; }

    private MimeType(string value)
    {
        Value = value;
    }

    public static Result<MimeType, Error> Create(string input)
    {
        var mimeType = input.Trim().ToLower();

        if (_types.Any(t => t.Value == mimeType) == false)
        {
            return Errors.General.ValueIsInvalid("mime type");
        }

        if (string.IsNullOrEmpty(mimeType) || mimeType.Length > Constants.Default.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("mime type");

        return new MimeType(mimeType);
    }

    public static Result<MimeType, Error> Parse (string fileName)
    {
        var fileExtension = Path.GetExtension(fileName).Replace(".", "").ToLower();

        foreach (var type in _types)
        {
            var typeParts = type.Value.Split("/");

            if(fileExtension == typeParts.Last())
            {
                return Create(type.Value);
            }
        }

        return Errors.General.ValueIsInvalid("mime type");
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(MimeType type) =>
        type.Value;
}