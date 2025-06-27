using Microsoft.EntityFrameworkCore;
using Selah.Core.Models.Entities.Identity;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository;

public class UserSessionRepository(AppDbContext dbContext) : IUserSessionRepository
{
    public async Task<UserSessionEntity?> GetUserSessionAsync(Guid userId)
    {
        return await dbContext.UserSessions
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    /// <summary>
    /// Revoke any existing sessions for a given user before issuing a new one
    /// </summary>
    /// <param name="userSession"></param>
    public async Task IssueSession(UserSessionEntity userSession)
    {
        await RevokeSessionsByUser(userSession.UserId, false);
        await dbContext.UserSessions.AddAsync(userSession);
        await dbContext.SaveChangesAsync();
    }

    public async Task RevokeSessionsByUser(Guid userId, bool autocommit)
    {
        List<UserSessionEntity>? userSessions =
            await (dbContext.UserSessions.Where(x => x.UserId == userId)).ToListAsync();

        if (userSessions.Any())
        {
            dbContext.UserSessions.RemoveRange(userSessions);
            if (autocommit)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}