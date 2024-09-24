using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.AccountManagement.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;