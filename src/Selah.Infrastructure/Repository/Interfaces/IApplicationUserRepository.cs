using Selah.Core.Models.Entities.ApplicationUser;

namespace Selah.Infrastructure.Repository.Interfaces;

public interface IApplicationUserRepository
{
    Task<ApplicationUserEntity?> GetUserByIdAsync(Guid userId);

    Task<ApplicationUserEntity?> GetUserByEmail(string emailHash);
}