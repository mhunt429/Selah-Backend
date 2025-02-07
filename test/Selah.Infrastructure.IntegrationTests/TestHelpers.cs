using Microsoft.EntityFrameworkCore;
using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Core.Models.Sql.UserAccount;
using Selah.Infrastructure.Repository;

namespace Selah.Infrastructure.IntegrationTests;

public static class TestHelpers
{
    public static string TestConnectionString { get; } =
        "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres";

    public static AppDbContext BuildTestDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(TestConnectionString)
            .Options;

        return new AppDbContext(options);
    }

    public static SelahDbConnectionFactory TestDbFactory { get; } = new SelahDbConnectionFactory(TestConnectionString);

    /// <summary>
    /// It's simple, most records need a user, and each user needs a single account
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="accountId"></param>
    /// <param name="repository"></param>
    public static async Task<(UserAccountSql, ApplicationUserSql)> SetUpBaseRecords(Guid userId, Guid accountId, IRegistrationRepository repository)
    {
        UserAccountSql account = new UserAccountSql
        {
            Id = accountId,
            AccountName = "AccountName",
            CreatedOn = DateTimeOffset.UtcNow,
            AppLastChangedBy = userId
        };

        ApplicationUserSql user = new ApplicationUserSql
        {
            AppLastChangedBy = userId,
            AccountId = accountId,
            Id = userId,
            EncryptedEmail = "email",
            Username = Guid.NewGuid().ToString().Substring(0, 19),
            Password = "password",
            EncryptedName = "FirstName|LastName",
            EncryptedPhone = "123-123-1234",
            LastLoginIp = "127.0.0.1",
        };
        
       
        await repository.RegisterAccount(account, user);
        
        return (account, user);
    }

    public static async Task TearDownBaseRecords(Guid userId, Guid accountId, IBaseRepository repository)
    {
        string deleteUserSql = "DELETE FROM app_user WHERE id = @id";
        string deleteAccountUser = "DELETE FROM account WHERE id = @id";

        await repository.DeleteAsync(deleteUserSql, new { id = userId });
        await repository.DeleteAsync(deleteAccountUser, new { id = accountId });
    }
}