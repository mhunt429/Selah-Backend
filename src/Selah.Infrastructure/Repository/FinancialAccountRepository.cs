using Dapper;
using Microsoft.EntityFrameworkCore;
using Selah.Core.Constants;
using Selah.Core.Models.Entities.FinancialAccount;
using Selah.Infrastructure.Extensions;

namespace Selah.Infrastructure.Repository;

public class FinancialAccountRepository : IFinancialAccountRepository
{
    private readonly AppDbContext _dbContext;

    public FinancialAccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountEntity> accounts)
    {
        await _dbContext.FinancialAccounts.AddRangeAsync(accounts);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<long> AddAccountAsync(FinancialAccountEntity account)
    {
        await _dbContext.FinancialAccounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
        return account.Id;
    }

    public async Task<IEnumerable<FinancialAccountEntity?>> GetAccountsAsync(Guid userId)
    {
        return await _dbContext.FinancialAccounts.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<FinancialAccountEntity?> GetAccountByIdAsync(Guid userId, long id)
    {
        return await _dbContext.FinancialAccounts
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task<bool> UpdateAccount(FinancialAccountEntity account)
    {
        _dbContext.FinancialAccounts.Update(account);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAccountAsync(FinancialAccountEntity account)
    {
        _dbContext.Remove(account);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}