using MediatR;
using Selah.Core.Models.Plaid;
using Selah.Core.Models.Sql.AccountConnector;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.AccountConnector;

public class ExchangeLinkToken
{
    public class Command : TokenExchangeHttpRequest, IRequest<bool>
    {
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly IAccountConnectorRepository _accountConnectorRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IPlaidHttpService _plaidHttpService;

        public Handler(IAccountConnectorRepository accountConnectorRepository, ICryptoService cryptoService,
            IPlaidHttpService plaidHttpService)
        {
            _accountConnectorRepository = accountConnectorRepository;
            _cryptoService = cryptoService;
            _plaidHttpService = plaidHttpService;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            bool retVal = false;
            PlaidTokenExchangeResponse? plaidTokenExchangeResponse =
                await _plaidHttpService.ExchangePublicToken(request.UserId,
                    request.PublicToken);

            if (plaidTokenExchangeResponse == null)
            {
                return retVal;
            }
            //If we get a token back from Plaid, save the record into the account_connector table

            AccountConnectorSql dataToSave = new AccountConnectorSql
            {
                AppLastChangedBy = request.UserId,
                UserId = request.UserId,
                InstitutionId = request.InstitutionId,
                InstitutionName = request.InstitutionName,
                DateConnected = DateTime.UtcNow,
                EncryptedAccessToken = _cryptoService.Encrypt(plaidTokenExchangeResponse.AccessToken),
                TransactionSyncCursor = ""
            };

            var newId = await _accountConnectorRepository.InsertAccountConnectorRecord(dataToSave);

            return newId > 1;
        }
    }
}