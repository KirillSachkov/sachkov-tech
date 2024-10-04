using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;