namespace SachkovTech.Domain.Modules;

public record IssueId
{
    private IssueId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static IssueId NewModuleId() => new(Guid.NewGuid());

    public static IssueId Empty() => new(Guid.Empty);

    public static IssueId Create(Guid id) => new(id);
}