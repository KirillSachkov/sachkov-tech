using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Entities;

public class Issue : Entity<IssueId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private List<FileId> _files = [];

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
        IEnumerable<FileId>? files = null) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        Experience = experience;
        _files = files?.ToList() ?? [];
    }

    public Experience Experience { get; private set; } = default!;
    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public Position Position { get; private set; }

    public LessonId LessonId { get; private set; }

    public IReadOnlyList<FileId> Files => _files;


    public void UpdateFiles(IEnumerable<FileId> files)
    {
        _files = files.ToList();
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