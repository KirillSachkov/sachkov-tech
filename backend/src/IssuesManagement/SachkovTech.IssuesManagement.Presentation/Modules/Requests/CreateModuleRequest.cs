using SachkovTech.IssuesManagement.Application.Commands.Create;

namespace SachkovTech.IssuesManagement.Presentation.Modules.Requests;

public record CreateModuleRequest(string Title, string Description)
{
    public CreateModuleCommand ToCommand() => new (Title, Description);
}