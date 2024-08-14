using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Modules;

public sealed class Module : Entity<ModuleId>
{
    private readonly List<Issue> _issues = [];

    // ef core
    private Module(ModuleId id) : base(id)
    {
    }

    private Module(ModuleId moduleId, Title title, Description description)
        : base(moduleId)
    {
        Title = title;
        Description = description;
    }

    public Title Title { get; private set; } = default!;

    public Description Description { get; private set; } = default!;

    public IReadOnlyList<Issue> Issues => _issues;

    public int GetNumberOfIssues() => _issues.Count;

    public void AddIssue(Issue issue)
    {
        // валидация
        _issues.Add(issue);
    }

    public static Result<Module> Create(ModuleId moduleId, Title title, Description description)
    {
        return new Module(moduleId, title, description);
    }
}