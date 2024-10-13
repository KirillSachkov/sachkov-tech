namespace SachkovTech.Accounts.Contracts.Requests;

public record RefreshTokensRequest(string AccessToken, Guid RefreshToken);