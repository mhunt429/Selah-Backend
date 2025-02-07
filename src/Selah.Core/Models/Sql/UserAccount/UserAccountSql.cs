using System.ComponentModel.DataAnnotations;

namespace Selah.Core.Models.Sql.UserAccount;

public class UserAccountSql : BaseAuditFields
{
    [Key] public Guid Id { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string AccountName { get; set; }
}