using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Infrastructure.Services.Connector;

public class PlaidAccountBalanceImportService
{
    private readonly IPlaidHttpService _plaidHttpService;
    private readonly IFinancialAccountRepository _financialAccountRepository;

    public PlaidAccountBalanceImportService(IPlaidHttpService plaidHttpService,
        IFinancialAccountRepository financialAccountRepository)
    {
        _plaidHttpService = plaidHttpService;
        _financialAccountRepository = financialAccountRepository;
    }
    
}