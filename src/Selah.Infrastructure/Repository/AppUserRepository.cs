using Microsoft.EntityFrameworkCore;
using Selah.Core.Constants;
using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository;

public class AppUserRepository : IApplicationUserRepository
{
    private readonly AppDbContext _appDbContext;

    public AppUserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ApplicationUserEntity?> GetUserByIdAsync(Guid id)
    {
        return await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ApplicationUserEntity?> GetUserByEmail(string emailHash)
    {
        return await _appDbContext.ApplicationUsers
            .FirstOrDefaultAsync(x => x.EmailHash == emailHash);
    }
}