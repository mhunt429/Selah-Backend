using Dapper;
using Selah.Core.Constants;
using Selah.Core.Models.Sql.AccountConnector;
using Selah.Infrastructure.Extensions;

namespace Selah.Infrastructure.Repository;

public class AccountConnectorRepository: IAccountConnectorRepository
{
    private readonly AppDbContext _dbContext;

    public AccountConnectorRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Insert into account_connector upon successful connection through Plaid or Finicity
    /// </summary>
    public async Task<long> InsertAccountConnectorRecord(AccountConnectorSql account)
    {
       await _dbContext.AccountConnectors.AddAsync(account);
       await _dbContext.SaveChangesAsync();
       return account.Id;
    }
}