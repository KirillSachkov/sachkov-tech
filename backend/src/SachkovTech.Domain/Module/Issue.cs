namespace SachkovTech.Domain.Module;

public class Issue
{
    public Guid Id { get; set; }

    public Guid LessonId { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public Issue? ParentIssue { get; set; }

    public List<Issue> SubIssues { get; set; } = [];

    public List<File> Files { get; set; } = [];
}