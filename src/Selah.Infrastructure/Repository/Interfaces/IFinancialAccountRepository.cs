using Selah.Core.Models.Entities.FinancialAccount;

namespace Selah.Infrastructure.Repository;

public interface IFinancialAccountRepository
{
    Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountEntity> accounts);

    Task<long> AddAccountAsync(FinancialAccountEntity account);

    Task<IEnumerable<FinancialAccountEntity?>> GetAccountsAsync(Guid userId);

    Task<FinancialAccountEntity?> GetAccountByIdAsync(Guid userId, long id);

    Task<bool> UpdateAccount(FinancialAccountEntity account);

    Task<bool> DeleteAccountAsync(FinancialAccountEntity account);
}