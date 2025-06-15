using Selah.Core.Models.Entities.Identity;

namespace Selah.Infrastructure.Repository.Interfaces;

public interface IUserSessionRepository
{
    Task<UserSessionEntity?> GetUserSessionAsync(Guid userId);
    
    Task IssueSession(UserSessionEntity userSession);
    
    Task RevokeSession(Guid userId);
    
    Task UpdateSession(UserSessionEntity userSession);
}