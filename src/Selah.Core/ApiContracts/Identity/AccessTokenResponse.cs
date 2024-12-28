namespace Selah.Core.ApiContracts.Identity;

public class AccessTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;
    
    public DateTimeOffset AccessTokenExpiration { get; set; }
    
    public DateTimeOffset RefreshTokenExpiration { get; set; }
}