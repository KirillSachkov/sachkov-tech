using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

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

    public void UpdateMainInfo(Title title, Description description)
    {
        Title = title;
        Description = description;
    }

    public void Delete()
    {
        if (_isDeleted) return;

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

    public UnitResult<Error> AddIssues(IEnumerable<Issue> issues)
    {
        // валидация
        _issues.AddRange(issues);

        return Result.Success<Error>();
    }
}