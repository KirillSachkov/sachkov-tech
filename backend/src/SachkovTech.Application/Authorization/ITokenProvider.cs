using SachkovTech.Application.Authorization.DataModels;

namespace SachkovTech.Application.Authorization;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}