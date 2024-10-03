using SachkovTech.Core.Abstraction;

namespace SachkovTech.Accounts.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;