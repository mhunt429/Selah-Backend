using Dapper;
using Selah.Core.Constants;
using Selah.Core.Models.Sql.FinancialAccount;
using Selah.Infrastructure.Extensions;

namespace Selah.Infrastructure.Repository;

public class FinancialAccountRepository : IFinancialAccountRepository
{
    private readonly IBaseRepository _baseRepository;

    public FinancialAccountRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountSqlInsert> accounts)
    {
        IEnumerable<DynamicParameters> parameters = accounts.Select(account => account.ConvertToSnakecase());

        await _baseRepository.PerformTransaction(parameters, SqlQueries.InsertIntoFinancialAccount);
    }

    public async Task<long> AddAccountAsync(FinancialAccountSqlInsert account)
    {
        return await _baseRepository.AddAsync<long>(SqlQueries.InsertIntoFinancialAccount,
            account.ConvertToSnakecase());
    }

    public async Task<IEnumerable<FinancialAccountSql>> GetAccountsAsync(Guid userId)
    {
        return await _baseRepository.GetAllAsync<FinancialAccountSql>(SqlQueries.GetFinancialAccountsByUserId,
            new { user_id = userId });
    }

    public async Task<FinancialAccountSql> GetAccountByIdAsync(Guid userId, long id)
    {
        return await _baseRepository.GetFirstOrDefaultAsync<FinancialAccountSql>(SqlQueries.GetFinancialAccountsById,
            new { id = id, user_id = userId });
    }

    public async Task<bool> UpdateAccountBalance(FinancialAccountBalanceUpdate accountBalanceUpdate)
    {
        return await _baseRepository.UpdateAsync(SqlQueries.UpdateAccountBalance,
            accountBalanceUpdate.ConvertToSnakecase());
    }

    public async Task<bool> DeleteAccountAsync(Guid userId, long id)
    {
        return await _baseRepository.DeleteAsync(SqlQueries.DeleteFinancialAccount, new { id = id, user_id = userId });
    }
}