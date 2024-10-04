using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.Create;

public record CreateModuleCommand(string Title, string Description) : ICommand;