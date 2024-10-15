using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.RestoreIssue;

public record RestoreIssueCommand(Guid ModuleId, Guid IssueId) : ICommand;