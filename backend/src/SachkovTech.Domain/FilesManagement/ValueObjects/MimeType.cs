using System.Net.Http.Headers;
using System.Net.Mime;
using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class MimeType : ValueObject
{
    public static readonly MimeType Png = new(MediaTypeNames.Image.Png);
    public static readonly MimeType Jpeg = new(MediaTypeNames.Image.Jpeg);
    public static readonly MimeType Gif = new(MediaTypeNames.Image.Gif);
    public static readonly MimeType Svg = new(MediaTypeNames.Image.Svg);
    public static readonly MimeType Icon = new(MediaTypeNames.Image.Icon);
    public static readonly MimeType Bmp = new(MediaTypeNames.Image.Bmp);

    public static readonly MimeType Pdf = new(MediaTypeNames.Application.Pdf);
    public static readonly MimeType Json = new(MediaTypeNames.Application.Json);
    public static readonly MimeType Zip = new(MediaTypeNames.Application.Zip);
    public static readonly MimeType Rtf = new(MediaTypeNames.Application.Rtf);
    public static readonly MimeType Xml = new(MediaTypeNames.Application.Xml);

    public static readonly MimeType Mp4 = new("video/mp4");
    public static readonly MimeType Mpeg = new("video/mpeg");
    public static readonly MimeType Webm = new("video/webm");

    public string Value { get; }

    private MimeType(string value)
    {
        Value = value;
    }

    public Result<MimeType, Error> Create(string type)
    {
        if (string.IsNullOrEmpty(type) || type.Length > Constants.Default.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("MimeType");

        return new MimeType(type);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(MimeType type) =>
        type.Value;
}