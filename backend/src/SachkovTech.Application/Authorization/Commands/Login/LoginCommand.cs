using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.Authorization.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;