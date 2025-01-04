using Selah.Core.Constants;
using Selah.Core.Models.Sql.Registration;

namespace Selah.Infrastructure.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly IBaseRepository _baseRepository;

    public RegistrationRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<Guid> RegisterAccount(RegistrationSql registrationSql)
    {
        Guid accountId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        (string, object) account = (SqlQueries.InsertIntoAccount,
            new
            {
                original_insert = DateTimeOffset.UtcNow,
                last_update = DateTimeOffset.UtcNow,
                id = accountId,
                app_last_changed_by = userId,
                date_created = DateTimeOffset.UtcNow,
                account_name = registrationSql.AccountName
            });

        (string, object) user = (SqlQueries.InsertIntoAppUser, new
        {
            app_last_changed_by = userId,
            original_insert = DateTimeOffset.UtcNow,
            last_update = DateTimeOffset.UtcNow,
            id = userId,
            account_id = accountId,
            created_date = registrationSql.CreatedDate,
            encrypted_email = registrationSql.EncryptedEmail,
            username = registrationSql.Username,
            password = registrationSql.Password,
            encrypted_name = registrationSql.EncryptedName,
            encrypted_phone = registrationSql.EncryptedPhone,
            last_login = registrationSql.LastLogin,
            last_login_ip = registrationSql.LastLoginIp,
            phone_verified = registrationSql.PhoneVerified,
            email_verified = registrationSql.EmailVerified,
            email_hash = registrationSql.EmailHash,
        });

        List<(string, object)> dbTransactions = new List<(string, object)>();

        dbTransactions.Add(account);
        dbTransactions.Add(user);
        await _baseRepository.PerformTransaction(dbTransactions);

        return userId;
    }
}