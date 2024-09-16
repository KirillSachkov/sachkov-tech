namespace SachkovTech.Application.IssueManagement.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid ModuleId, string Title, string Description);