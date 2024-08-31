namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public record IssueFile
{
    public IssueFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public FilePath PathToStorage { get; }
}