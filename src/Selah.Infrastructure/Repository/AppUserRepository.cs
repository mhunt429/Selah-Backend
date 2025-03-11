using Selah.Core.Constants;
using Selah.Core.Models.Entities.ApplicationUser;

namespace Selah.Infrastructure.Repository;

public class AppUserRepository : IApplicationUserRepository
{
    private readonly IBaseRepository _baseRepository;

    public AppUserRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<ApplicationUserEntity?> GetUserByIdAsync(Guid id)
    {
        return await _baseRepository.GetFirstOrDefaultAsync<ApplicationUserEntity>(SqlQueries.GetUserById, new { id });
    }

    public async Task<ApplicationUserEntity> GetUserByEmail(string emailHash)
    {
        return await _baseRepository.GetFirstOrDefaultAsync<ApplicationUserEntity>(SqlQueries.GetUserByEmail,
            new { email_hash = emailHash });
    }
}