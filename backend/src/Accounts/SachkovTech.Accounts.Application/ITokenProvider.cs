using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Application;

public interface ITokenProvider
{
    Task<string> GenerateAccessToken(User user);
}