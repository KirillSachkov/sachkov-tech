using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SachkovTech.Accounts.Application;
using SachkovTech.Accounts.Domain;
using SachkovTech.Accounts.Infrastructure.IdentityManagers;
using SachkovTech.Core.Models;
using SachkovTech.Core.Options;

namespace SachkovTech.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly PermissionManager _permissionManager;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options, PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
        _jwtOptions = options.Value;
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles.Select(r => new Claim(CustomClaims.Role, r.Name ?? string.Empty));

        var permissions = await _permissionManager.GetUserPermissionCodes(user.Id);
        var permissionClaims = permissions.Select(p => new Claim(CustomClaims.Permission, p));

        Claim[] claims =
        [
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
        ];

        claims = claims
            .Concat(roleClaims)
            .Concat(permissionClaims)
            .ToArray();

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);

        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return stringToken;
    }
}