using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid ModuleId, string Title, string Description) : ICommand;