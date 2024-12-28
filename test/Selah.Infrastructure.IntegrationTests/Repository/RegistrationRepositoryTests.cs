using FluentAssertions;
using Selah.Core.Models.Sql.Registration;
using Selah.Infrastructure.Repository;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class RegistrationRepositoryTests
{
    private readonly IBaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);

    private readonly RegistrationRepository _repository;

    public RegistrationRepositoryTests()
    {
        _repository = new RegistrationRepository(_baseRepository);
    }

    [Fact]
    public async Task Register_ShouldSaveAccountAndUserRecord()
    {
        RegistrationSql registrationSql = new RegistrationSql
        {
            EncryptedEmail = "email",
            Username = Guid.NewGuid().ToString().Substring(0,19),
            Password = "password",
            EncryptedName = "FirstName|LastName",
            EncryptedPhone = "123-123-1234",
            LastLoginIp = "127.0.0.1",
            AccountName = "AccountName",
        };

        Guid userId = await _repository.RegisterAccount(registrationSql);
        
        userId.Should().NotBe(Guid.Empty);
    }
}