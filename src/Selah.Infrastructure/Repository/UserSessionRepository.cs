using Microsoft.EntityFrameworkCore;
using Selah.Core.Models.Entities.Identity;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository;

public class UserSessionRepository(AppDbContext dbContext): IUserSessionRepository
{
    public async Task<UserSessionEntity?> GetUserSessionAsync(Guid userId)
    {
        return await dbContext.UserSessions
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task IssueSession(UserSessionEntity userSession)
    {
        await dbContext.UserSessions.AddAsync(userSession);
        await dbContext.SaveChangesAsync();
    }

    public async Task RevokeSession(Guid userId)
    {
        UserSessionEntity? userSession =
            await dbContext.UserSessions.FirstOrDefaultAsync(x => x.SessionId == userId);

        if (userSession != null)
        {
            dbContext.UserSessions.Remove(userSession);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateSession(UserSessionEntity userSession)
    {
        dbContext.UserSessions.Update(userSession);
        await dbContext.SaveChangesAsync();
    }
}