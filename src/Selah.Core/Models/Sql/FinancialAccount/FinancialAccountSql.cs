namespace Selah.Core.Models.Sql.FinancialAccount;

public class FinancialAccountSql
{
    public long Id { get; set; }
    
    public Guid UserId { get; set; }

    public string ExternalId { get; set; } = "";
    
    public decimal CurrentBalance { get; set; }

    public string AccountMask { get; set; } = "";
    
    public required  string DisplayName { get; set; }

    public string OfficialName { get; set; } = "";
    
    public required string Subtype { get; set; }
    
}

public class FinancialAccountSqlInsert
{
    public required string ExternalId { get; set; }
    
    public decimal CurrentBalance { get; set; }
   
    public required string AccountMask { get; set; }
    
    public required  string DisplayName { get; set; }
    
    public required string OfficialName { get; set; }
    
    public required string Subtype { get; set; }
}

public class FinancialAccountBalanceUpdate
{
    public decimal CurrentBalance { get; set; } 
}