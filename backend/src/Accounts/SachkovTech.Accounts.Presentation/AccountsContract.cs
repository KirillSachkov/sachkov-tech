﻿using CSharpFunctionalExtensions;
using SachkovTech.Accounts.Application.Commands.Register;
using SachkovTech.Accounts.Contracts;
using SachkovTech.Accounts.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Presentation;

public class AccountsContract(RegisterUserHandler registerUserHandler) : IAccountsContract
{
    public async Task<UnitResult<ErrorList>> RegisterUser(
        RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        return await registerUserHandler.Handle(
            new RegisterUserCommand(request.Email, request.UserName, request.Password),
            cancellationToken);
    }
}