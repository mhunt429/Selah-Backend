using Selah.Core.Models.Entities.ApplicationUser;

namespace Selah.Infrastructure.Repository;

public interface IApplicationUserRepository
{
    Task<ApplicationUserEntity?> GetUserByIdAsync(Guid userId);

    Task<ApplicationUserEntity?> GetUserByEmail( string encryptedEmail);
}