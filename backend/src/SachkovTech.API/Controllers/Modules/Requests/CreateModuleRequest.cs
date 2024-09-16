using SachkovTech.Application.IssueManagement.Commands.Create;

namespace SachkovTech.API.Controllers.Modules.Requests;

public record CreateModuleRequest(string Title, string Description)
{
    public CreateModuleCommand ToCommand() => new (Title, Description);
}