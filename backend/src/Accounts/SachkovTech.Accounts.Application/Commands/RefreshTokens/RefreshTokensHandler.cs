using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Models;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;

public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshTokensCommand command, CancellationToken cancellationToken = default)
    {
        var oldRefreshSession = await _refreshSessionManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);

        if (oldRefreshSession.IsFailure)
            return oldRefreshSession.Error.ToErrorList();

        if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
        {
            return Errors.Tokens.ExpiredToken().ToErrorList();
        }

        var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken, cancellationToken);
        if (userClaims.IsFailure)
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }

        var userIdString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Errors.General.Failure().ToErrorList();
        }

        if (oldRefreshSession.Value.UserId != userId)
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }

        var userJtiString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Jti)?.Value;
        if (!Guid.TryParse(userJtiString, out var userJtiGuid))
        {
            return Errors.General.Failure().ToErrorList();
        }

        if (oldRefreshSession.Value.Jti != userJtiGuid)
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }

        _refreshSessionManager.Delete(oldRefreshSession.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = await _tokenProvider
            .GenerateAccessToken(oldRefreshSession.Value.User, cancellationToken);

        var refreshToken = await _tokenProvider
            .GenerateRefreshToken(oldRefreshSession.Value.User, accessToken.Jti, cancellationToken);

        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}