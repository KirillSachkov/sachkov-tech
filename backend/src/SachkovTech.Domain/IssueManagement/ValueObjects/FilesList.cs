namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public record FilesList
{
    private FilesList()
    {
    }

    public FilesList(IEnumerable<File> files)
    {
        Files = files.ToList();
    }

    public IReadOnlyList<File> Files { get; } = [];
}