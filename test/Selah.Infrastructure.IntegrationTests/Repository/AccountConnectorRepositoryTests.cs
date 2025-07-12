using FluentAssertions;
using Selah.Core.Models.Entities.AccountConnector;
using Selah.Infrastructure.Repository;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class AccountConnectorRepositoryTests : IAsyncLifetime
{
    private readonly BaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);
    
    private readonly AppDbContext _dbContext =  TestHelpers.BuildTestDbContext();
    
    private readonly IAccountConnectorRepository _accountConnectorRepository;

    private Guid _accountId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();

    public AccountConnectorRepositoryTests()
    {
        _accountConnectorRepository = new AccountConnectorRepository(_dbContext);
    }

    [Fact]
    public async Task InsertAccountConnectorRecord_ShouldSaveRecord()
    {
        AccountConnectorEntity data = new AccountConnectorEntity
        {
            AppLastChangedBy = _userId,
            UserId = _userId,
            InstitutionId = "123",
            InstitutionName = "Morgan Stanley",
            DateConnected = DateTimeOffset.UtcNow,
            EncryptedAccessToken = "token",
            TransactionSyncCursor = "",
            OriginalInsert = DateTimeOffset.UtcNow,
        };
        await _accountConnectorRepository.InsertAccountConnectorRecord(data);

        var queryResult =
            await _baseRepository.GetFirstOrDefaultAsync<AccountConnectorEntity>(
                "SELECT * FROM account_connector WHERE user_id = @user_id", new { user_id = _userId });

        queryResult.Should().NotBeNull();
        queryResult.UserId.Should().Be(_userId);
        queryResult.EncryptedAccessToken.Should().Be(data.EncryptedAccessToken);
        queryResult.DateConnected.Should().BeAfter(DateTimeOffset.MinValue);
        queryResult.InstitutionId.Should().Be(data.InstitutionId);
        queryResult.InstitutionName.Should().Be(data.InstitutionName);
    }

    public async Task InitializeAsync()
    {
        var registrationRepository = new RegistrationRepository(_dbContext);
        await TestHelpers.SetUpBaseRecords(_accountId,_userId, registrationRepository);
    }

    public async Task DisposeAsync()
    {
        await TestHelpers.TearDownBaseRecords(_userId, _accountId, _baseRepository);

        string accountConnectorDelete = "DELETE FROM account_connector WHERE user_id = @user_id";
        await _baseRepository.DeleteAsync(accountConnectorDelete, new { user_id = _userId });
    }
}