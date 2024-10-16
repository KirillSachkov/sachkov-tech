using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Domain;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByNameAsync(Role.Participant);

        var user = User.CreateAdmin(command.UserName, command.Email, role!);

        await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("User created: {userName} a new account with password.", command.UserName);
            return Result.Success<ErrorList>();
        }


        var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return new ErrorList(errors);
    }
}