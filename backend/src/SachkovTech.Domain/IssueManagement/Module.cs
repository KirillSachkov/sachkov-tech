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

    public Result<Issue, Error> GetIssueByPosition(Position position)
    {
        var issue = _issues.FirstOrDefault(i => i.Position == position);
        if (issue is null)
            return Errors.General.NotFound();

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

    public UnitResult<Error> MoveIssue(IssueId issueId, Position newPosition)
    {
        var issueResult = GetIssueById(issueId);
        if (issueResult.IsFailure)
            return issueResult.Error;

        var issue = issueResult.Value;
        var currentPosition = issue.Position;

        if (currentPosition.Value == newPosition.Value)
            return Result.Success<Error>();

        var adjustedPositionResult = AdjustNewPositionIfOutOfBounds(newPosition);
        if (adjustedPositionResult.IsFailure)
            return adjustedPositionResult.Error;

        newPosition = adjustedPositionResult.Value;
        
        var moveResult = MoveIssuesBetweenPositions(currentPosition, newPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        issue.SetPosition(newPosition);

        return Result.Success<Error>();
    }
    
    private Result<Position, Error> AdjustNewPositionIfOutOfBounds(Position newPosition)
    {
        if (newPosition.Value <= _issues.Count)
            return newPosition;
        
        var lastPosition = Position.Create(_issues.Count - 1);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;

    }
    
    private UnitResult<Error> MoveIssuesBetweenPositions(Position currentPosition, Position newPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var issuesToMove = _issues.Where(i => i.Position.Value >= newPosition.Value
                                                  && i.Position.Value < currentPosition.Value);

            foreach (var issueToMove in issuesToMove)
            {
                var result = issueToMove.MoveForward();
                if (result.IsFailure)
                    return result.Error;
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var issuesToMove = _issues.Where(i => i.Position.Value > currentPosition.Value
                                                  && i.Position.Value <= newPosition.Value);

            foreach (var issueToMove in issuesToMove)
            {
                var result = issueToMove.MoveBack();
                if (result.IsFailure)
                    return result.Error;
            }
        }

        return Result.Success<Error>();
    }
}