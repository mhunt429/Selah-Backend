using Selah.Core.Models.Sql.Registration;

namespace Selah.Infrastructure.Repository;

public interface IRegistrationRepository
{
    /// <summary>
    /// Returning simply userId due to the transactional nature of this. User creates an account,
    /// gets, a token on success
    /// </summary>
    Task<Guid> RegisterAccount(RegistrationSql registrationSql);
}