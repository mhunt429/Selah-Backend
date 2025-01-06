namespace Selah.Core.Models.Sql.AccountConnector;

public class AccountConnectorSql
{
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public required string InstitutionId { get; set; }

    public required string InstitutionName { get; set; }

    public required DateTimeOffset DateConnected { get; set; }

    public required string EncryptedAccessToken { get; set; }

    public required string TransactionSyncCursor { get; set; }
}

public class AccountConnectorInsert
{
    public DateTimeOffset OriginalInsert { get; set; }

    public DateTimeOffset LastUpdate { get; set; }

    public Guid AppLastChangedBy { get; set; }

    public Guid UserId { get; set; }

    public required string InstitutionId { get; set; }

    public required string InstitutionName { get; set; }

    public required DateTimeOffset DateConnected { get; set; }

    public required string EncryptedAccessToken { get; set; }

    public required string TransactionSyncCursor { get; set; }
}