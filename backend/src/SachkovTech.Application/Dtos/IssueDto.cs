namespace SachkovTech.Application.Dtos;

public class IssueDto
{
    public Guid Id { get; init; }
    
    public Guid ModuleId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Position { get; private set; }

    public Guid LessonId { get; private set; }

    public Guid? ParentId { get; private set; }

    public string Files { get; private set; } = string.Empty;
}