using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public class FilePath : ValueObject
{
    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        // валидация на доступные расширения файлов

        var fullPath = path + extension;

        return new FilePath(fullPath);
    }

    public static Result<FilePath, Error> Create(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(fullPath);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Path;
    }
}