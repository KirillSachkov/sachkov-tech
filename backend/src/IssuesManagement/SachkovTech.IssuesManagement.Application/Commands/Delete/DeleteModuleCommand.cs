using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesManagement.Application.Commands.Delete;

public record DeleteModuleCommand(Guid ModuleId) : ICommand;