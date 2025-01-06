using FluentAssertions;
using Selah.Core.Models.Sql.AccountConnector;
using Selah.Infrastructure.Repository;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class AccountConnectorRepositoryTests : IAsyncLifetime
{
    private readonly IBaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);
    private readonly IAccountConnectorRepository _accountConnectorRepository;

    private Guid _accountId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();

    public AccountConnectorRepositoryTests()
    {
        _accountConnectorRepository = new AccountConnectorRepository(_baseRepository);
    }

    [Fact]
    public async Task InsertAccountConnectorRecord_ShouldSaveRecord()
    {
        AccountConnectorInsert data = new AccountConnectorInsert
        {
            AppLastChangedBy = _userId,
            UserId = _userId,
            OriginalInsert = DateTimeOffset.UtcNow,
            LastUpdate = DateTimeOffset.UtcNow,
            InstitutionId = "123",
            InstitutionName = "Morgan Stanley",
            DateConnected = DateTimeOffset.UtcNow,
            EncryptedAccessToken = "token",
            TransactionSyncCursor = ""
        };
        await _accountConnectorRepository.InsertAccountConnectorRecord(data);

        var queryResult =
            await _baseRepository.GetFirstOrDefaultAsync<AccountConnectorSql>(
                "SELECT * FROM account_connector WHERE user_id = @user_id", new { user_id = _userId });

        queryResult.Should().NotBeNull();
        queryResult.UserId.Should().Be(_userId);
        queryResult.EncryptedAccessToken.Should().Be(data.EncryptedAccessToken);
        queryResult.DateConnected.Should().Be(data.DateConnected);
        queryResult.InstitutionId.Should().Be(data.InstitutionId);
        queryResult.InstitutionName.Should().Be(data.InstitutionName);
    }

    public async Task InitializeAsync()
    {
        var registrationRepository = new RegistrationRepository(_baseRepository);
        await TestHelpers.SetUpBaseRecords(_userId, _accountId, registrationRepository);
    }

    public async Task DisposeAsync()
    {
        await TestHelpers.TearDownBaseRecords(_userId, _accountId, _baseRepository);

        string accountConnectorDelete = "DELETE FROM account_connector WHERE user_id = @user_id";
        await _baseRepository.DeleteAsync(accountConnectorDelete, new { user_id = _userId });
    }
}