using Selah.Core.Models.Sql.AccountConnector;

namespace Selah.Infrastructure.Repository;

public interface IAccountConnectorRepository
{
    Task<long> InsertAccountConnectorRecord(AccountConnectorSql account);
}