using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Commands.Delete;

public record DeleteModuleCommand(Guid ModuleId) : ICommand;