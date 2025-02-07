using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Core.Models.Sql.UserAccount;

namespace Selah.Infrastructure.Repository;

public interface IRegistrationRepository
{
    /// <summary>
    /// Returning simply userId due to the transactional nature of this. User creates an account,
    /// gets, a token on success
    /// </summary>
    Task<Guid> RegisterAccount(UserAccountSql userAccount, ApplicationUserSql user);
}