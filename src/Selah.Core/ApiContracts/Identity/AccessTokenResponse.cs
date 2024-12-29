namespace Selah.Core.ApiContracts.Identity;

public class AccessTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;
    
    public DateTime AccessTokenExpiration { get; set; }
    
    public DateTime RefreshTokenExpiration { get; set; }
}