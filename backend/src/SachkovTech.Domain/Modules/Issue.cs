using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public class Issue : Entity<IssueId>
{
    //ef core
    private Issue(IssueId id) : base(id)
    {
    }

    public Guid? LessonId { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public Issue? ParentIssue { get; set; }

    public List<Issue> SubIssues { get; set; } = [];

    public IssueDetails? Details { get; private set; }
}