namespace Selah.Core.Models;

public class AppRequestContext
{
    public string UserId { get; set; } = string.Empty;  
    public string TraceId { get; set; } = string.Empty;  
    public string IpAddress { get; set; }= string.Empty;  
}