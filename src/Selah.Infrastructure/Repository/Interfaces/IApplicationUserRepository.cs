using Selah.Core.Models.Sql.ApplicationUser;

namespace Selah.Infrastructure.Repository;

public interface IApplicationUserRepository
{
    Task<ApplicationUserSql?> GetUserByIdAsync(Guid userId);

    Task<ApplicationUserSql?> GetUserByEmail( string encryptedEmail);
}