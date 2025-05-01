using Selah.Core.Models.Entities.FinancialAccount;

namespace Selah.Infrastructure.Repository.Interfaces;

public interface IFinancialAccountRepository
{
    Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountEntity> accounts);

    Task<Guid> AddAccountAsync(FinancialAccountEntity account);

    Task<IEnumerable<FinancialAccountEntity?>> GetAccountsAsync(Guid userId);

    Task<FinancialAccountEntity?> GetAccountByIdAsync(Guid userId, Guid id);

    Task<bool> UpdateAccount(FinancialAccountEntity account);

    Task<bool> DeleteAccountAsync(FinancialAccountEntity account);
}