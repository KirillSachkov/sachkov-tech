using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Commands.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description,
    int Experience) : ICommand;