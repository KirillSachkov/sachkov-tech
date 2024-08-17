using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public record File
{
    private File(string pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public string PathToStorage { get; }

    public static Result<File, Error> Create(string pathToStorage)
    {
        if (string.IsNullOrWhiteSpace(pathToStorage))
            return Errors.General.ValueIsInvalid("PathToStorage");

        return new File(pathToStorage);
    }
}