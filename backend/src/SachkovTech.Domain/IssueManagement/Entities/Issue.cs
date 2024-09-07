using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueManagement.Entities;

public class Issue : Shared.Entity<IssueId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<Issue> _subIssues = [];

    //ef core
    private Issue(IssueId id) : base(id)
    {
    }

    public Issue(
        IssueId id,
        Title title,
        Description description,
        LessonId lessonId,
        Issue? parentIssue,
        ValueObjectList<IssueFile>? files) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        ParentIssue = parentIssue;
        Files = files ?? new ValueObjectList<IssueFile>([]);
    }

    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public Position Position { get; private set; }

    public LessonId LessonId { get; private set; }

    public Issue? ParentIssue { get; private set; }
    public IReadOnlyList<Issue> SubIssues => _subIssues;

    public ValueObjectList<IssueFile> Files { get; private set; }

    public void UpdateFiles(ValueObjectList<IssueFile> files) =>
        Files = files;

    public void SetPosition(Position position) =>
        Position = position;

    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }
}