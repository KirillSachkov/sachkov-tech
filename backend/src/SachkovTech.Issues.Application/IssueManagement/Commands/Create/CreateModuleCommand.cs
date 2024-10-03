using SachkovTech.Core.Abstraction;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.Create;

public record CreateModuleCommand(string Title, string Description) : ICommand;