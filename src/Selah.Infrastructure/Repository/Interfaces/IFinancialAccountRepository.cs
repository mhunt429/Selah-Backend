using Selah.Core.Models.Sql.FinancialAccount;

namespace Selah.Infrastructure.Repository;

public interface IFinancialAccountRepository
{
    Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountSql> accounts);

    Task<long> AddAccountAsync(FinancialAccountSql account);

    Task<IEnumerable<FinancialAccountSql?>> GetAccountsAsync(Guid userId);

    Task<FinancialAccountSql?> GetAccountByIdAsync(Guid userId, long id);

    Task<bool> UpdateAccount(FinancialAccountSql account);

    Task<bool> DeleteAccountAsync(FinancialAccountSql account);
}