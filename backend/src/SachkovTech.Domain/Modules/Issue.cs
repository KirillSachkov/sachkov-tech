using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public class Issue : Shared.Entity<IssueId>, ISoftDeletable
{
    private bool _isDeleted = false;

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
        Issue? parentIssue) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        ParentIssue = parentIssue;
        // Details = details;
    }

    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public LessonId LessonId { get; private set; }

    public Issue? ParentIssue { get; private set; }
    public IReadOnlyList<Issue> SubIssues => _subIssues;

    public FilesList? FilesList { get; private set; }

    public void UpdateFilesList(FilesList filesList) =>
        FilesList = filesList;

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
        Issue? parentIssue)
    {
        return new Issue(id, title, description, lessonId, parentIssue);
    }

    public void Delete()
    {
        if (_isDeleted == false)
        {
            _isDeleted = true;
        }
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }
}