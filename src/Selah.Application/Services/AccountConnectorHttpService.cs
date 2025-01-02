using Selah.Application.Commands.AccountConnector;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.Core.Models.Plaid;

namespace Selah.Application.Services;

public class AccountConnectorHttpService : IAccountConnectorHttpService
{
    private readonly ICreateLinkTokenCommand _createLinkTokenCommand;

    public AccountConnectorHttpService(ICreateLinkTokenCommand createLinkTokenCommand)
    {
        _createLinkTokenCommand = createLinkTokenCommand;
    }

    public async Task<BaseHttpResponse<PlaidLinkToken>> CreateLinkToken(Guid userId)
    {
        PlaidLinkToken? linkToken = await _createLinkTokenCommand.CreateLinkToken(userId);
        {
            return new BaseHttpResponse<PlaidLinkToken>
            {
                StatusCode = linkToken != null ? 200 : 400,
                Data = linkToken,
            };
        }
    }
}