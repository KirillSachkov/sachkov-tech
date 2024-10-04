using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesManagement.Application.Commands.Create;

public record CreateModuleCommand(string Title, string Description) : ICommand;