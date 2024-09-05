namespace SachkovTech.Application.Modules.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description,
    IEnumerable<CreateFileCommand> FileCommands);

public record CreateFileCommand(Stream Content, string FileName);