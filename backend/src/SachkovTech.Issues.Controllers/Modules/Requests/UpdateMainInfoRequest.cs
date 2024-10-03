using SachkovTech.Issues.Application.IssueManagement.Commands.UpdateMainInfo;

namespace SachkovTech.Issues.Controllers.Modules.Requests;

public record UpdateMainInfoRequest(
    string Title,
    string Description)
{
    public UpdateMainInfoCommand ToCommand(Guid moduleId) =>
        new(moduleId, Title, Description);
}