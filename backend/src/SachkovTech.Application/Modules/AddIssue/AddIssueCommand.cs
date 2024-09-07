namespace SachkovTech.Application.Modules.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description);