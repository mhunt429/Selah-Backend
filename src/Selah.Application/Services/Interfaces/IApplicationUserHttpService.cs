using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.Identity;

namespace Selah.Application.Services.Interfaces;

public interface IApplicationUserHttpService
{
    Task<BaseHttpResponse<AccessTokenResponse>> LoginUser(LoginRequest loginRequest);
}
