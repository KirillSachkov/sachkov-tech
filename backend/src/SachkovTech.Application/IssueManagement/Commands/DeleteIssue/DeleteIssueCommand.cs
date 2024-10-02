using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Commands.DeleteIssue;

public record DeleteIssueCommand(Guid ModuleId, Guid IssueId) : ICommand;
