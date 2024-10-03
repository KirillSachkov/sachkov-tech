using SachkovTech.Core.Abstraction;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid ModuleId, string Title, string Description) : ICommand;