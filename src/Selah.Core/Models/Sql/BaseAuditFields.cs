namespace Selah.Core.Models.Sql;

public  class BaseAuditFields
{
    public DateTimeOffset OriginalInsert { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdate { get; private set; } = DateTimeOffset.UtcNow;
    public required Guid AppLastChangedBy { get; set; }
}