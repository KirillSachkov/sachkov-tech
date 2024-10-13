using System.Security.Claims;
using CSharpFunctionalExtensions;
using SachkovTech.Accounts.Application.Models;
using SachkovTech.Accounts.Domain;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application;

public interface ITokenProvider
{
    Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken);
    Task<Guid> GenerateRefreshToken(User user, Guid accessTokenJti, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(
        string jwtToken, CancellationToken cancellationToken);
}