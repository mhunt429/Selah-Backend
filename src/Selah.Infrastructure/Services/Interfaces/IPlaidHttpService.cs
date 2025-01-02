using Selah.Core.Models.Plaid;

namespace Selah.Infrastructure.Services.Interfaces;

public interface IPlaidHttpService
{
    Task<PlaidLinkToken?> GetLinkToken(Guid userId);

    Task<PlaidTokenExchangeResponse?> ExchangePublicToken(Guid userId, string publicToken);
}