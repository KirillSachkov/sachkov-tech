namespace SachkovTech.Core.Options;

public class JwtOptions
{
    public const string JWT = nameof(JWT);
    
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public string ExpiredMinutesTime { get; init; } = string.Empty;
}