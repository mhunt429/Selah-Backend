using FluentAssertions;
using Selah.Core.Models.Sql.Registration;
using Selah.Infrastructure.Repository;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class RegistrationRepositoryTests : IAsyncLifetime
{
    private readonly IBaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);

    private readonly RegistrationRepository _repository;

    private readonly Guid accountId = Guid.NewGuid();
    private Guid userId = Guid.NewGuid();

    public RegistrationRepositoryTests()
    {
        _repository = new RegistrationRepository(_baseRepository);
    }

    [Fact]
    public async Task Register_ShouldSaveAccountAndUserRecord()
    {
        RegistrationSql registrationSql = new RegistrationSql
        {
            AccountId = accountId,
            UserId = userId,
            EncryptedEmail = "email",
            Username = Guid.NewGuid().ToString().Substring(0, 19),
            Password = "password",
            EncryptedName = "FirstName|LastName",
            EncryptedPhone = "123-123-1234",
            LastLoginIp = "127.0.0.1",
            AccountName = "AccountName",
        };

        userId = await _repository.RegisterAccount(registrationSql);

        userId.Should().NotBe(Guid.Empty);
    }

    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        string deleteUserSql = "DELETE FROM app_user WHERE id = @id";
        string deleteAccountUser = "DELETE FROM account WHERE id = @id";

        await _baseRepository.DeleteAsync(deleteUserSql, new { id = userId });
        await _baseRepository.DeleteAsync(deleteAccountUser, new { id = accountId });
    }
}