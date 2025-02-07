using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Core.Models.Sql.UserAccount;

namespace Selah.Infrastructure.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly AppDbContext _dbContext;

    public RegistrationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> RegisterAccount(UserAccountSql userAccount, ApplicationUserSql user)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                _dbContext.UserAccounts.Add(userAccount);
                await _dbContext.SaveChangesAsync();

                _dbContext.ApplicationUsers.Add(user);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return userAccount.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }
}