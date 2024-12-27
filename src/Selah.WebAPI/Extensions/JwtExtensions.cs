using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace Selah.WebAPI.Extensions;

public static class JwtExtensions
{
    public static void RegisterJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["selah-api"],
                ValidAudience = configuration["selah-api"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["SecurityConfig:JwtSecret"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }
}