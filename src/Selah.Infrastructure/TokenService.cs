using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Selah.Core.Configuration;

namespace Selah.Infrastructure;

public class TokenService
{
    private readonly SecurityConfig _securityConfig;

    public TokenService(SecurityConfig securityConfig)
    {
        _securityConfig = securityConfig;
    }

    public string GenerateAccessToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Convert.FromBase64String(_securityConfig.JwtSecret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", userId)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_securityConfig.AccessTokenExpiryMinutes),
            Issuer = "selah-api",
            Audience = "selah-api",
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(string userId)
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}