using CSharpFunctionalExtensions;
using SachkovTech.Accounts.Domain;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken);

    void Delete(RefreshSession refreshSession);
}