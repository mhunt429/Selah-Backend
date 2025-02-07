using Dapper;
using Microsoft.EntityFrameworkCore;
using Selah.Core.Constants;
using Selah.Core.Models.Sql.FinancialAccount;
using Selah.Infrastructure.Extensions;

namespace Selah.Infrastructure.Repository;

public class FinancialAccountRepository : IFinancialAccountRepository
{
    private readonly AppDbContext _dbContext;

    public FinancialAccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ImportFinancialAccountsAsync(IEnumerable<FinancialAccountSql> accounts)
    {
        await _dbContext.FinancialAccounts.AddRangeAsync(accounts);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<long> AddAccountAsync(FinancialAccountSql account)
    {
        await _dbContext.FinancialAccounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
        return account.Id;
    }

    public async Task<IEnumerable<FinancialAccountSql?>> GetAccountsAsync(Guid userId)
    {
        return await _dbContext.FinancialAccounts.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<FinancialAccountSql?> GetAccountByIdAsync(Guid userId, long id)
    {
        return await _dbContext.FinancialAccounts.Where(x => x.UserId == userId && x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAccount(FinancialAccountSql account)
    {
        _dbContext.FinancialAccounts.Update(account);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAccountAsync(FinancialAccountSql account)
    {
        _dbContext.Remove(account);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}