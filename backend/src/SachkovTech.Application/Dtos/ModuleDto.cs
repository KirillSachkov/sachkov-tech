namespace SachkovTech.Application.Dtos;

public class ModuleDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public IssueDto[] Issues { get; init; } = [];
}