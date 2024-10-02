using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Commands.UpdatePosition;

public record UpdateIssuePositionCommand(Guid ModuleId, Guid IssueId, int NewPosition) : ICommand;