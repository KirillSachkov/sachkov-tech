using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Commands.Delete;

public record DeleteModuleCommand(Guid ModuleId) : ICommand;