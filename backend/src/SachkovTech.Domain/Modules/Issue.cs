using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Modules;

public class Issue : Shared.Entity<IssueId>
{
    private readonly List<Issue> _subIssues = [];

    //ef core
    private Issue(IssueId id) : base(id)
    {
    }

    private Issue(
        IssueId id,
        Title title,
        Description description,
        LessonId lessonId,
        Issue? parentIssue,
        IssueDetails details) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        ParentIssue = parentIssue;
        Details = details;
    }

    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public LessonId LessonId { get; private set; }

    public Issue? ParentIssue { get; private set; }
    public IReadOnlyList<Issue> SubIssues => _subIssues;

    public IssueDetails? Details { get; private set; }

    public void AddIssueDetails(IssueDetails details) =>
        Details = details;

    public void AddSubIssue(Issue issue)
    {
        // logic and validation
        _subIssues.Add(issue);
    }

    public static Result<Issue> Create(
        IssueId id,
        Title title,
        Description description,
        LessonId lessonId,
        Issue? parentIssue,
        IssueDetails details)
    {
        return new Issue(id, title, description, lessonId, parentIssue, details);
    }
}