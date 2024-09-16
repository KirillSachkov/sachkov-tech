namespace SachkovTech.Application.Dtos;

public class IssueDto
{
    public Guid Id { get; init; }

    public Guid ModuleId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Position { get; set; }

    public Guid LessonId { get; set; }

    public Guid? ParentId { get; set; }

    public IssueFileDto[] Files { get; set; } = null!;
}

public class IssueFileDto
{
    public string PathToStorage { get; set; } = string.Empty;
    public int Size { get; set; }
}