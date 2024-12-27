using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selah.Core.Models;

namespace Selah.WebAPI.Extensions;

public static class HttpRequestExtensions
{
    public static AppRequestContext? GetAppRequestContext(this HttpRequest request)
    {
        var forwardedFor = request.Headers["X-Forwarded-For"].FirstOrDefault();
        var ipAddress = !string.IsNullOrWhiteSpace(forwardedFor)
            ? forwardedFor.Split(',').FirstOrDefault()?.Trim()
            : request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        // Get the "sub" claim
        var bearerToken = request.Headers["Authorization"]
            .FirstOrDefault(h => h.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
            ?.Substring("Bearer ".Length);
        
        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            request.Cookies.TryGetValue("x_token", out bearerToken);
        }
        
        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            return null; // No token found
        }
        
        string? userId = GetUserIdFromToken(bearerToken);

        return new AppRequestContext
        {
            IpAddress = ipAddress,
            UserId = userId,
            TraceId = request.HttpContext.TraceIdentifier,
        };
    }
    
    private static string? GetUserIdFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            }
        }
        catch
        {
            return null;
        }

        return null; // Return null if extraction fails
    }
}