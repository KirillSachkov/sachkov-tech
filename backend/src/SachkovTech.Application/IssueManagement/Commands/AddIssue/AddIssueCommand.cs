namespace SachkovTech.Application.IssueManagement.Commands.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description);