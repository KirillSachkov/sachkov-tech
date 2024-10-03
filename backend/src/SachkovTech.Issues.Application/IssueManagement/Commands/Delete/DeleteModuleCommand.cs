using SachkovTech.Core.Abstraction;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.Delete;

public record DeleteModuleCommand(Guid ModuleId) : ICommand;