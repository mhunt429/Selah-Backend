using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;

namespace Selah.Application.Services.Interfaces;

public interface IRegistrationHttpService
{
    Task<BaseHttpResponse<AccessTokenResponse>> RegisterAccount(AccountRegistrationRequest request);
}