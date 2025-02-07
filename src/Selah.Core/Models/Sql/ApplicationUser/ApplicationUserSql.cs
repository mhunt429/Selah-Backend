using System.ComponentModel.DataAnnotations;

namespace Selah.Core.Models.Sql.ApplicationUser;

public class ApplicationUserSql: BaseAuditFields
{
    [Key]
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public string EncryptedEmail { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string EncryptedName { get; set; } = string.Empty;

    public string EncryptedPhone { get; set; } = string.Empty;

    public DateTimeOffset LastLogin { get; set; } = DateTimeOffset.UtcNow;

    public string LastLoginIp { get; set; } = string.Empty;


    public bool PhoneVerified { get; set; }

    public bool EmailVerified { get; set; }
    
    public string EmailHash { get; set; } = string.Empty;
}