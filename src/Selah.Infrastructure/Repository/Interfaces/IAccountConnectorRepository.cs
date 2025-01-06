using Selah.Core.Models.Sql.AccountConnector;

namespace Selah.Infrastructure.Repository;

public interface IAccountConnectorRepository
{
    Task InsertAccountConnectorRecord(AccountConnectorInsert accountConnectorInsert);
}