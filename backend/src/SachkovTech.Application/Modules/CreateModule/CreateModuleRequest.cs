namespace SachkovTech.Application.Modules.CreateModule;

public record CreateModuleRequest(int Years, FullNameDto FullName, string Title, string Description);

public record FullNameDto(string FirstName, string SecondName);