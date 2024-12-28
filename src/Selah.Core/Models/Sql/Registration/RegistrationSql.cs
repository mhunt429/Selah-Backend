namespace Selah.Core.Models.Sql.Registration;

public class RegistrationSql
{
    public Guid UserId { get; set; }

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    public string EncryptedEmail { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string EncryptedName { get; set; } = string.Empty;

    public string EncryptedPhone { get; set; } = string.Empty;

    public DateTimeOffset LastLogin { get; set; } = DateTimeOffset.UtcNow;

    public string LastLoginIp { get; set; } = string.Empty;


    public bool PhoneVerified { get; set; }

    public bool EmailVerified { get; set; }

    public string AccountName { get; set; } = string.Empty;
}