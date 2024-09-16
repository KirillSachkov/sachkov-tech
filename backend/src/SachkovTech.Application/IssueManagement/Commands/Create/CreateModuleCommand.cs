using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Commands.Create;

public record CreateModuleCommand(string Title, string Description) : ICommand;