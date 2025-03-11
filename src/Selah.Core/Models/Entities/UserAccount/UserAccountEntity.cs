using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selah.Core.Models.Entities.UserAccount;

[Table("user_account")]
public class UserAccountEntity : BaseAuditFields
{
    [Key] [Column("id")] public Guid Id { get; set; }
    [Column("created_on")] public DateTimeOffset CreatedOn { get; set; }
    [Column("account_name")] public string? AccountName { get; set; }
}