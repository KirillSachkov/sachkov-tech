using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;