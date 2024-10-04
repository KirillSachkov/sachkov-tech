using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Commands.UpdateIssueMainInfo;

public record UpdateIssueMainInfoCommand(
    Guid ModuleId,
    Guid IssueId,
    string Title,
    string Description,
    int Experience): ICommand;