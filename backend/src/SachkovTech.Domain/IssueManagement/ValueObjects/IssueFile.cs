using System.Text.Json.Serialization;

namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public record IssueFile
{
    [JsonConstructor]
    public IssueFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public FilePath PathToStorage { get; }
}