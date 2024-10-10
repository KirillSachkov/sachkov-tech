using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssueSolving.Application.Commands.TakeOnWork;

public record TakeOnWorkCommand(Guid UserId, Guid IssueId) : ICommand;