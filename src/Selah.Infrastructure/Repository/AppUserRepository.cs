using Selah.Core.Constants;
using Selah.Core.Models.Sql.ApplicationUser;

namespace Selah.Infrastructure.Repository;

public class AppUserRepository : IApplicationUserRepository
{
    private readonly IBaseRepository _baseRepository;

    public AppUserRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<ApplicationUserSql> GetUserByIdAsync(Guid id)
    {
        return await _baseRepository.GetFirstOrDefaultAsync<ApplicationUserSql>(SqlQueries.GetUserById, new { id });
    }
}