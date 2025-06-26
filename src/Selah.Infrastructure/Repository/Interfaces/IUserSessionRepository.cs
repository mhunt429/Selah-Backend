using Selah.Core.Models.Entities.Identity;

namespace Selah.Infrastructure.Repository.Interfaces;

public interface IUserSessionRepository
{
    Task<UserSessionEntity?> GetUserSessionAsync(Guid userId);
    
    Task IssueSession(UserSessionEntity userSession);
    
    Task RevokeSessionsByUser(Guid userId, bool autocommit);
    
    Task UpdateSession(UserSessionEntity userSession);
}