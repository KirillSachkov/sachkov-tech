using SachkovTech.Application.IssueManagement.Commands.UpdateMainInfo;

namespace SachkovTech.API.Controllers.Modules.Requests;

public record UpdateMainInfoRequest(
    string Title,
    string Description)
{
    public UpdateMainInfoCommand ToCommand(Guid moduleId) =>
        new(moduleId, Title, Description);
}