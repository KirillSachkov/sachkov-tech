using SachkovTech.Accounts.Application.Commands.Login;

namespace SachkovTech.Accounts.Contracts.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginCommand ToCommand() => new(Email, Password);
};