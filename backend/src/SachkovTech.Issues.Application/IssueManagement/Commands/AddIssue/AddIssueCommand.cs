using SachkovTech.Core.Abstraction;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description) : ICommand;