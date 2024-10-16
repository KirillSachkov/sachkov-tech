using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssueSolving.Application.Commands.StopWorking;

public record StopWorkingCommand(Guid UserIssueId, Guid UserId) : ICommand;