using Selah.Core.ApiContracts.Identity;

namespace Selah.Infrastructure.Services.Interfaces;

public interface ITokenService
{
   AccessTokenResponse GenerateAccessToken(string userId);
}