namespace Selah.Core.Configuration;

public class SecurityConfig
{
    public string JwtSecret { get; set; }
    
    public string HashIdSalt { get; set; }
    
    public string CryptoSecret { get; set; }    
    
    public int AccessTokenExpiryMinutes { get; set; }    
    
    public int RefreshTokenExpiryDays { get; set; }  
}