using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selah.Core.Models.Sql.AccountConnector;

public class AccountConnectorSql: BaseAuditFields
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public required string InstitutionId { get; set; }

    public required string InstitutionName { get; set; }

    public required DateTimeOffset DateConnected { get; set; }

    public required string EncryptedAccessToken { get; set; }

    public required string TransactionSyncCursor { get; set; }
    
    //Set this field for the 3rd party webhooks
    public string ExternalEventId { get; set; }
}

