using Selah.Core.Models.Plaid;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Commands.AccountConnector;

public interface ICreateLinkTokenCommand
{
    Task<PlaidLinkToken?> Handle(Guid userId);
}

public class CreateLinkTokenCommand : ICreateLinkTokenCommand
{
    private readonly IPlaidHttpService _plaidHttpService;

    public CreateLinkTokenCommand(IPlaidHttpService plaidHttpService)
    {
        _plaidHttpService = plaidHttpService;
    }

    public async Task<PlaidLinkToken?> Handle(Guid userId)
    {
        return await _plaidHttpService.GetLinkToken(userId);
    }
}