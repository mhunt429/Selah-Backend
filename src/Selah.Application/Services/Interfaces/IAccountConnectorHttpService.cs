using Selah.Core.ApiContracts;
using Selah.Core.Models.Plaid;

namespace Selah.Application.Services.Interfaces;

public interface IAccountConnectorHttpService
{
    Task<BaseHttpResponse<PlaidLinkToken>> CreateLinkToken(Guid id);
}