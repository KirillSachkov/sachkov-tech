using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.Authorization.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;