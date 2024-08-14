namespace SachkovTech.Application.Modules.CreateModule;

public record UpdateModuleCommand(Guid id, UpdateModuleDto UpdateModuleDto);

public record UpdateModuleDto(string Title, string Description);

public record GetAllModulesResponse(List<ModuleDto> modules);

public record ModuleDto(Guid id, string Title, string Description);