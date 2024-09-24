using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.AccountManagement.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;