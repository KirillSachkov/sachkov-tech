using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Entities;

public class Issue : Entity<IssueId>, ISoftDeletable
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
        Issue? parentIssue) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        ParentIssue = parentIssue;
    }

    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public Position Position { get; private set; }

    public LessonId LessonId { get; private set; }

    public Issue? ParentIssue { get; private set; }
    public IReadOnlyList<Issue> SubIssues => _subIssues;

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

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public void Move(Position newPosition) =>
        Position = newPosition;
}