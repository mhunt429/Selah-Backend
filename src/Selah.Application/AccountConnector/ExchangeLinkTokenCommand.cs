using Selah.Core.Models.Plaid;
using Selah.Core.Models.Sql.AccountConnector;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Commands.AccountConnector;

public interface IExchangeLinkTokenCommand
{
    Task<bool> Handle(TokenExchangeHttpRequest tokenExchange);
}

public class ExchangeLinkTokenCommand: IExchangeLinkTokenCommand
{
    private readonly IAccountConnectorRepository _accountConnectorRepository;
    private readonly ICryptoService _cryptoService;
    private readonly IPlaidHttpService _plaidHttpService;

    public ExchangeLinkTokenCommand(IAccountConnectorRepository accountConnectorRepository,
        ICryptoService cryptoService, IPlaidHttpService plaidHttpService)
    {
        _accountConnectorRepository = accountConnectorRepository;
        _cryptoService = cryptoService;
        _plaidHttpService = plaidHttpService;
    }

    public async Task<bool> Handle(TokenExchangeHttpRequest tokenExchange)
    {
        bool success = false;
        PlaidTokenExchangeResponse? plaidTokenExchangeResponse =
            await _plaidHttpService.ExchangePublicToken(tokenExchange.UserId, tokenExchange.PublicToken);

        if (plaidTokenExchangeResponse == null)
        {
            return success;
        }
        //If we get a token back from Plaid, save the record into the account_connector table

        AccountConnectorInsert dataToSave = new AccountConnectorInsert
        {
            OriginalInsert = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
            AppLastChangedBy = tokenExchange.UserId,
            UserId = tokenExchange.UserId,
            InstitutionId = tokenExchange.InstitutionId,
            InstitutionName = tokenExchange.InstitutionName,
            DateConnected = DateTime.UtcNow,
            EncryptedAccessToken = _cryptoService.Encrypt(plaidTokenExchangeResponse.AccessToken),
            TransactionSyncCursor = ""
        };

        await _accountConnectorRepository.InsertAccountConnectorRecord(dataToSave);

        return true;
    }
}