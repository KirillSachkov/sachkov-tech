namespace SachkovTech.Application.Modules.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid ModuleId, string Title, string Description);