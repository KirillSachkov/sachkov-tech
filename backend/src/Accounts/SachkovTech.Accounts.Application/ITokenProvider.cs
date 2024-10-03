using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}