using Dapper;
using Selah.Core.Constants;
using Selah.Core.Models.Sql.AccountConnector;
using Selah.Infrastructure.Extensions;

namespace Selah.Infrastructure.Repository;

public class AccountConnectorRepository: IAccountConnectorRepository
{
    private readonly IBaseRepository _baseRepository;

    public AccountConnectorRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }


    /// <summary>
    /// Insert into account_connector upon successful connection through Plaid or Finicity
    /// </summary>
    public async Task<long> InsertAccountConnectorRecord(AccountConnectorInsert accountConnectorInsert)
    {
        DynamicParameters dataToSave = accountConnectorInsert.ConvertToSnakecase();

       return await _baseRepository.AddAsync<long>(SqlQueries.InsertIntoAccountConnector, dataToSave);
    }
}