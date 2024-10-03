using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueManagement.Entities;

public class Issue : CSharpFunctionalExtensions.Entity<IssueId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<Issue> _subIssues = [];

    private List<IssueFile> _files = [];

    //ef core navigation
    public Module Module { get; private set; }

    //ef core
    private Issue(IssueId id) : base(id)
    {
    }

    public Issue(
        IssueId id,
        Title title,
        Description description,
        LessonId lessonId,
        Experience experience,
        ValueObjectList<IssueFile>? files) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        Experience = experience;
        _files = files ?? new ValueObjectList<IssueFile>([]);
    }

    public Experience Experience { get; private set; } = default!;
    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public Position Position { get; private set; }

    public LessonId LessonId { get; private set; }


    public IReadOnlyList<IssueFile> Files => _files;

    public void UpdateFiles(List<IssueFile> files)
    {
        _files = files;
    }

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

    internal UnitResult<Error> UpdateMainInfo(
        Title title,
        Description description,
        LessonId lessonId,
        Experience experience)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        Experience = experience;

        return Result.Success<Error>();
    }
}