using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.UpdateIssueMainInfo;

public record UpdateIssueMainInfoCommand(
    Guid ModuleId,
    Guid IssueId,
    string Title,
    string Description,
    int Experience): ICommand;