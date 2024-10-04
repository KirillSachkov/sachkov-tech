
using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.DeleteIssue;

public record DeleteIssueCommand(Guid ModuleId, Guid IssueId) : ICommand;
