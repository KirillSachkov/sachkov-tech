using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesManagement.Application.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid ModuleId, string Title, string Description) : ICommand;