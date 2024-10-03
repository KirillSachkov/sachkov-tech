using SachkovTech.Core.Abstraction;

namespace SachkovTech.Accounts.Application.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;