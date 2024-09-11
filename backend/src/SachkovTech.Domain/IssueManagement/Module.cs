using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueManagement;

public sealed class Module : Shared.Entity<ModuleId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<Issue> _issues = [];

    // ef core
    private Module(ModuleId id) : base(id)
    {
    }

    public Module(ModuleId moduleId, Title title, Description description)
        : base(moduleId)
    {
        Title = title;
        Description = description;
    }

    public Title Title { get; private set; } = default!;

    public Description Description { get; private set; } = default!;

    public IReadOnlyList<Issue> Issues => _issues;

    public int GetNumberOfIssues() => _issues.Count;

    public Result<Issue, Error> GetIssueById(IssueId issueId)
    {
        var issue = _issues.FirstOrDefault(i => i.Id == issueId);
        if (issue is null)
            return Errors.General.NotFound(issueId.Value);

        return issue;
    }

    public void UpdateMainInfo(Title title, Description description)
    {
        Title = title;
        Description = description;
    }

    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;

        foreach (var issue in _issues)
            issue.Delete();
    }

    public void Restore()
    {
        if (!_isDeleted) return;

        _isDeleted = false;
        foreach (var issue in _issues)
            issue.Restore();
    }

    public UnitResult<Error> AddIssue(Issue issue)
    {
        var serialNumberResult = Position.Create(_issues.Count + 1);
        if (serialNumberResult.IsFailure)
            return serialNumberResult.Error;

        issue.SetPosition(serialNumberResult.Value);

        _issues.Add(issue);
        return Result.Success<Error>();
    }

    public UnitResult<Error> MoveIssue(Issue issue, Position newPosition)
    {
        var currentPosition = issue.Position;

        if (currentPosition == newPosition || _issues.Count == 1)
            return Result.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var moveResult = MoveIssuesBetweenPositions(newPosition, currentPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        issue.Move(newPosition);

        return Result.Success<Error>();
    }

    private UnitResult<Error> MoveIssuesBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition < currentPosition)
        {
            var issuesToMove = _issues.Where(i => i.Position >= newPosition
                                                  && i.Position < currentPosition);

            foreach (var issueToMove in issuesToMove)
            {
                var result = issueToMove.MoveForward();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        else if (newPosition > currentPosition)
        {
            var issuesToMove = _issues.Where(i => i.Position > currentPosition
                                                  && i.Position <= newPosition);

            foreach (var issueToMove in issuesToMove)
            {
                var result = issueToMove.MoveBack();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        
        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _issues.Count)
            return newPosition;

        var lastPosition = Position.Create(_issues.Count - 1);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }
}