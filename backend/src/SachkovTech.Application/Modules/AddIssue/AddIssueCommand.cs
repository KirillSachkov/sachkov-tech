namespace SachkovTech.Application.Modules.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description,
    IEnumerable<FileDto> Files);

public record FileDto(Stream Content, string FileName, string ContentType);