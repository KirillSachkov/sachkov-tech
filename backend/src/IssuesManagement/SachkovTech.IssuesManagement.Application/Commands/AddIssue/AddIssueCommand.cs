using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesManagement.Application.Commands.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description) : ICommand;