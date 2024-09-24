using SachkovTech.Application.AccountManagement.Commands.Login;

namespace SachkovTech.API.Controllers.Accounts.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginCommand ToCommand() => new (Email, Password);
};