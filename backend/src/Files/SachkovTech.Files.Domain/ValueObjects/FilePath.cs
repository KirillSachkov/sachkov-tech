using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

public class FilePath : ValueObject
{
    public string Value { get; }

    public string BucketName => GetBucketName();

    public string Prefix => GetPrefix();

    public string FileName => GetFileName();
    
    public string FileNameWithPrefix => Prefix + FileName;

    public string FileExtension => GetFileExtension();

    private FilePath(string value)
    {
        Value = value;
    }

    public static Result<FilePath, Error> Create(string filePath)
    {
        var pathParts = filePath.Split('/')
            .Where(p => string.IsNullOrWhiteSpace(p) == false);

        if(pathParts.Count() < 2)
            return Errors.General.ValueIsInvalid("file path");

        if (string.IsNullOrWhiteSpace(filePath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(string.Join("/", pathParts));
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(FilePath filePath) =>
        filePath.Value;

    private string GetBucketName()
    {
        var filePathParts = Value.Split('/');

        return filePathParts.First();
    }

    private string GetPrefix()
    {
        var filePathParts = Value.Split('/');

        var prefixParts = filePathParts.Take(new Range(1, filePathParts.Length - 1));

        return string.Join("/", prefixParts);
    }

    private string GetFileName()
    {
        var filePathParts = Value.Split('/');

        return filePathParts.Last();
    }

    private string GetFileExtension()
    {
        return Path.GetExtension(FileName);
    }
}