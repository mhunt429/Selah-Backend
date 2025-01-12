using Hangfire;
using Microsoft.Extensions.Logging;

namespace Selah.Infrastructure.RecurringJobs;

[AutomaticRetryAttribute(Attempts = 0)]
public class RecurringAccountBalanceUpdateJob
{
    private readonly ILogger<RecurringAccountBalanceUpdateJob> _logger;

    public RecurringAccountBalanceUpdateJob(ILogger<RecurringAccountBalanceUpdateJob> logger)
    {
        _logger = logger;
    }


    public void DoWork()
    {
        _logger.LogInformation("Recurring account balance update");
    }
}