using SachkovTech.Accounts.Application.Commands.Register;

namespace SachkovTech.Accounts.Contracts.Requests;

public record RegisterUserRequest(string Email, string UserName, string Password)
{
    public RegisterUserCommand ToCommand() => new (Email, UserName, Password);
};