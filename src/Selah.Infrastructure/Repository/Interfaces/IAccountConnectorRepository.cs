using Selah.Core.Models.Entities.AccountConnector;

namespace Selah.Infrastructure.Repository;

public interface IAccountConnectorRepository
{
    Task<long> InsertAccountConnectorRecord(AccountConnectorEntity account);
}