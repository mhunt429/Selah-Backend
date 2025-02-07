using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selah.Core.Models.Sql.FinancialAccount;

public class FinancialAccountSql: BaseAuditFields
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public long Id { get; set; }
    
    public long ConnectorId { get; set; }

    public Guid UserId { get; set; }

    public string ExternalId { get; set; } = "";

    public decimal CurrentBalance { get; set; }

    public string AccountMask { get; set; } = "";

    public required string DisplayName { get; set; }

    public string OfficialName { get; set; } = "";

    public required string Subtype { get; set; }
    
    public bool IsExternalApiImport { get; set; }
    
    public DateTimeOffset LastApiImportTime { get; set; }
}

