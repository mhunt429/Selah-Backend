using Selah.Application.Commands.AccountConnector;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.Core.Models.Plaid;

namespace Selah.Application.Services;

public class AccountConnectorHttpService : IAccountConnectorHttpService
{
    private readonly ICreateLinkTokenCommand _createLinkTokenCommand;
    private readonly IExchangeLinkTokenCommand _exchangeLinkTokenCommand;

    public AccountConnectorHttpService(ICreateLinkTokenCommand createLinkTokenCommand, IExchangeLinkTokenCommand exchangeLinkTokenCommand)
    {
        _createLinkTokenCommand = createLinkTokenCommand;
        _exchangeLinkTokenCommand = exchangeLinkTokenCommand;
    }

    public async Task<BaseHttpResponse<PlaidLinkToken>> CreateLinkToken(Guid userId)
    {
        PlaidLinkToken? linkToken = await _createLinkTokenCommand.Handle(userId);
        {
            return new BaseHttpResponse<PlaidLinkToken>
            {
                StatusCode = linkToken != null ? 200 : 400,
                Data = linkToken,
            };
        }
    }

    public async Task<bool> ExchangePublicToken(TokenExchangeHttpRequest request)
    {
     return await _exchangeLinkTokenCommand.Handle(request);
    }
}