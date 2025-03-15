using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Core.Models.Entities.UserAccount;

namespace Selah.Infrastructure.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly AppDbContext _dbContext;

    public RegistrationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> RegisterAccount(UserAccountEntity userAccount, ApplicationUserEntity user)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                _dbContext.UserAccounts.Add(userAccount);

                _dbContext.ApplicationUsers.Add(user);
                
                
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }
}