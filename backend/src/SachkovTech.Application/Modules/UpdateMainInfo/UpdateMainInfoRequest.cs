namespace SachkovTech.Application.Modules.UpdateMainInfo;

public record UpdateMainInfoRequest(Guid ModuleId, UpdateMainInfoDto Dto);
public record UpdateMainInfoDto(string Title, string Description);