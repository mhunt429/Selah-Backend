using Selah.Core.Models.Sql.FinancialAccount;

namespace Selah.Infrastructure.Repository;

public interface IFinancialAccountRepository
{
    Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountSqlInsert> accounts);

    Task<long> AddAccountAsync(FinancialAccountSqlInsert account);

    Task<IEnumerable<FinancialAccountSql>> GetAccountsAsync(Guid userId);

    Task<FinancialAccountSql> GetAccountByIdAsync(Guid userId, long id);

    Task<bool> UpdateAccountBalance(FinancialAccountBalanceUpdate accountBalanceUpdate);

    Task<bool> DeleteAccountAsync(Guid userId, long id);
}