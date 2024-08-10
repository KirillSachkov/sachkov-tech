using CSharpFunctionalExtensions;

namespace SachkovTech.Domain.Module;

public class Module
{
    private readonly List<Issue> _issues = [];

    // ef core
    private Module()
    {
    }

    private Module(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public IReadOnlyList<Issue> Issues => _issues;

    public int NumberOfIssues => Issues.Count;

    public void AddIssue(Issue issue)
    {
        // валидация
        _issues.Add(issue);
    }

    public static Result<Module> Create(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<Module>("Title can not be empty");

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Module>("Description can not be empty");

        var module = new Module(title, description);

        return Result.Success(module);
    }
}