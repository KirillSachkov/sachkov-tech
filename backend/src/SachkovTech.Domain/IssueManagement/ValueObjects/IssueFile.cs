using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public class IssueFile: ValueObject
{
    [JsonConstructor]
    public IssueFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public FilePath PathToStorage { get; }
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return PathToStorage;
    }
}