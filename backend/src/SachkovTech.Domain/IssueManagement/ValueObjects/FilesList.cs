namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public record FilesList
{
    private FilesList()
    {
    }

    public FilesList(IEnumerable<IssueFile> files)
    {
        Files = files.ToList();
    }

    public IReadOnlyList<IssueFile> Files { get; }
}