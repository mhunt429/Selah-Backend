using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Configuration;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Infrastructure;

public class TokenService: ITokenService
{
    private readonly SecurityConfig _securityConfig;

    public TokenService(SecurityConfig securityConfig)
    {
        _securityConfig = securityConfig;
    }

    public AccessTokenResponse GenerateAccessToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Convert.FromBase64String(_securityConfig.JwtSecret));

        DateTime accessTokenExpiration = DateTime.UtcNow.AddMinutes(_securityConfig.AccessTokenExpiryMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", userId)
            }),
            Expires =accessTokenExpiration,
            Issuer = "selah-api",
            Audience = "selah-api",
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        
        string accessToken = tokenHandler.WriteToken(token);
        string refreshToken = GenerateRefreshToken();

        return new AccessTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(_securityConfig.RefreshTokenExpiryDays)
        };
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}