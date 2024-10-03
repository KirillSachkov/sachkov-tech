using CSharpFunctionalExtensions;
using SachkovTech.Accounts.Application.Commands.Register;
using SachkovTech.Accounts.Contracts;
using SachkovTech.Accounts.Contracts.Requests;
using SachkovTech.Core;

namespace SachkovTech.Accounts.Presentation;

public class AccountsContract(RegisterUserHandler registerUserHandler) : IAccountsContract
{
    public async Task<UnitResult<ErrorList>> RegisterUser(
        RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        return await registerUserHandler.Handle(request.ToCommand(), cancellationToken);
    }
}