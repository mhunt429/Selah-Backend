using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Core.Models.Entities.UserAccount;

namespace Selah.Infrastructure.Repository;

public class RegistrationRepository(AppDbContext dbContext) : IRegistrationRepository
{
    public async Task<Guid> RegisterAccount(UserAccountEntity userAccount, ApplicationUserEntity user)
    {
        using (var transaction = await dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                dbContext.UserAccounts.Add(userAccount);

                dbContext.ApplicationUsers.Add(user);
                
                
                await dbContext.SaveChangesAsync();

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