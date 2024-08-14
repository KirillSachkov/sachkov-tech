using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public record File
{
    private File(string pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public string PathToStorage { get; }
    
    public static Result<File> Create(string pathToStorage)
    {
        if (string.IsNullOrWhiteSpace(pathToStorage))
        {
            return "PathToStorage cannot be empty";
        }

        return new File(pathToStorage);
    }
}