using MediatR;
using Selah.Core.Models.Plaid;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.AccountConnector;

public class CreateLinkToken
{
    public class Command : IRequest<PlaidLinkToken>
    {
        public Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Command, PlaidLinkToken?>
    {
        private readonly IPlaidHttpService _plaidHttpService;

        public Handler(IPlaidHttpService plaidHttpService)
        {
            _plaidHttpService = plaidHttpService;
        }
            
        public async  Task<PlaidLinkToken?> Handle(Command command, CancellationToken cancellationToken)
        {
            return await _plaidHttpService.GetLinkToken(command.UserId);
        }
    }
}

