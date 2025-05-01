using Selah.Core.Models;
using Selah.Core.Models.Entities.AccountConnector;

namespace Selah.Infrastructure.Repository;

public interface IAccountConnectorRepository
{
    Task<DbOperationResult> InsertAccountConnectorRecord(AccountConnectorEntity account);
}