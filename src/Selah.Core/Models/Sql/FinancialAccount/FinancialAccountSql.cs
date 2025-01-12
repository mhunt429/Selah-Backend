namespace Selah.Core.Models.Sql.FinancialAccount;

public class FinancialAccountSql
{
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

public class FinancialAccountSqlInsert: BaseAuditFields
{
    public Guid UserId { get; set; }
   
    public long ConnectorId { get; set; }
    
    public required string ExternalId { get; set; }

    public decimal CurrentBalance { get; set; }

    public required string AccountMask { get; set; }

    public required string DisplayName { get; set; }

    public required string OfficialName { get; set; }

    public required string Subtype { get; set; }
    
    public bool IsExternalApiImport { get; set; }
    
    public DateTimeOffset LastApiImportTime { get; set; }
}

public class FinancialAccountBalanceUpdate
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTimeOffset LastUpdate => DateTimeOffset.Now;
}