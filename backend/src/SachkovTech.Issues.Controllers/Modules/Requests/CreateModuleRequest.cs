using SachkovTech.Issues.Application.IssueManagement.Commands.Create;

namespace SachkovTech.Issues.Controllers.Modules.Requests;

public record CreateModuleRequest(string Title, string Description)
{
    public CreateModuleCommand ToCommand() => new (Title, Description);
}