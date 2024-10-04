using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description,
    int Experience) : ICommand;