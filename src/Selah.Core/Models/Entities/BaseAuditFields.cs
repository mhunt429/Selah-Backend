using System.ComponentModel.DataAnnotations.Schema;

namespace Selah.Core.Models.Entities;

public class BaseAuditFields
{
    [Column("original_insert")] public DateTimeOffset OriginalInsert { get; private set; } = DateTimeOffset.UtcNow;

    [Column("last_update")] public DateTimeOffset LastUpdate { get; private set; } = DateTimeOffset.UtcNow;

    [Column("app_last_changed_by")] public required Guid AppLastChangedBy { get; set; }
}