using FluentAssertions;
using Selah.Core.Models.Entities.AccountConnector;
using Selah.Core.Models.Entities.FinancialAccount;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class FinancialAccountRepositoryTests : IAsyncLifetime
{
    private readonly IBaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);
    
    private readonly AppDbContext _dbContext =  TestHelpers.BuildTestDbContext();

    private readonly IFinancialAccountRepository _financialAccountRepository;
    private readonly IAccountConnectorRepository _accountConnectorRepository;

    private Guid _accountId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();

    private Guid _connectorId = Guid.NewGuid();

    public FinancialAccountRepositoryTests()
    {
        _financialAccountRepository = new FinancialAccountRepository(_dbContext);
        _accountConnectorRepository = new AccountConnectorRepository(_dbContext);
    }

    [Fact]
    public async Task ImportFinancialAccountsAsync_ShouldInsertMultipleAccounts()
    {
        var data = new List<FinancialAccountEntity>
        {
            new FinancialAccountEntity
            {
                AppLastChangedBy = _userId,
                UserId = _userId,
                ExternalId = "1234",
                AccountMask = "***111",
                CurrentBalance = 100,
                DisplayName = "My Checking",
                OfficialName = "RBC Personal Checking",
                Subtype = "Checking",
                IsExternalApiImport = true,
                LastApiSyncTime = DateTimeOffset.UtcNow,
                ConnectorId = _connectorId,
                OriginalInsert = DateTimeOffset.UtcNow,
            },
            new FinancialAccountEntity
            {
                AppLastChangedBy = _userId,
                UserId = _userId,
                ExternalId = "4321",
                AccountMask = "***4321",
                DisplayName = "My Saving",
                CurrentBalance = 500,
                OfficialName = "RBC Personal Savings",
                Subtype = "Savings",
                IsExternalApiImport = true,
                LastApiSyncTime = DateTimeOffset.UtcNow,
                ConnectorId = _connectorId,
                OriginalInsert = DateTimeOffset.UtcNow,
            },
        };

        await _financialAccountRepository.ImportFinancialAccountsAsync(data);

        var result = await _financialAccountRepository.GetAccountsAsync(_userId);

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddAccountAsync_ShouldInsertNewAccount()
    {
        var account = new FinancialAccountEntity
        {
            AppLastChangedBy = _userId,
            UserId = _userId,
            ExternalId = "4321",
            AccountMask = "***4321",
            DisplayName = "Vanguard Trust 401k",
            CurrentBalance = 500,
            OfficialName = "Vanguard Total Trust 401k",
            Subtype = "Retirement",
            IsExternalApiImport = true,
            LastApiSyncTime = DateTimeOffset.UtcNow,
            ConnectorId = _connectorId,
            OriginalInsert = DateTimeOffset.UtcNow,
        };

        Guid newAccountId = await _financialAccountRepository.AddAccountAsync(account);

        var result = await _financialAccountRepository.GetAccountByIdAsync(_userId, newAccountId);
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.UserId.Should().Be(_userId);
        result.ExternalId.Should().Be("4321");
        //result.AccountMask.Should().Be("***4321");
        result.DisplayName.Should().Be("Vanguard Trust 401k");
        result.CurrentBalance.Should().Be(500);
        result.OfficialName.Should().Be("Vanguard Total Trust 401k");
        result.Subtype.Should().Be("Retirement");
        result.IsExternalApiImport.Should().BeTrue();
        result.LastApiSyncTime.Should().BeAfter(DateTimeOffset.MinValue);
    }

    [Fact(Skip = "Trying to fix some weird change tracking within this test")]
    public async Task UpdateAccountAsync_ShouldUpdateAccount()
    {
        var account = new FinancialAccountEntity
        {
            AppLastChangedBy = _userId,
            UserId = _userId,
            ExternalId = "4321",
            AccountMask = "***4321",
            DisplayName = "Vanguard Trust 401k",
            CurrentBalance = 500,
            OfficialName = "Vanguard Total Trust 401k",
            Subtype = "Retirement",
            IsExternalApiImport = true,
            LastApiSyncTime = DateTimeOffset.UtcNow,
            ConnectorId = _connectorId,
            OriginalInsert = DateTimeOffset.UtcNow,
        };

        Guid newAccountId = await _financialAccountRepository.AddAccountAsync(account);

        var accountUpdate = new FinancialAccountEntity
        {
            Id = newAccountId,
            UserId = _userId,
            CurrentBalance = 1000,
            DisplayName = "Vanguard Trust 401k",
            Subtype = "Retirement",
            AppLastChangedBy = _userId,
            OriginalInsert = DateTimeOffset.UtcNow,
        };

        await _financialAccountRepository.UpdateAccount(accountUpdate);
        var result = await _financialAccountRepository.GetAccountByIdAsync(_userId, newAccountId);
        result.Should().NotBeNull();
        result.CurrentBalance.Should().Be(1000);
    }

    [Fact]
    public async Task DeleteAccountAsync_ShouldDeleteAccount()
    {
        var account = new FinancialAccountEntity
        {
            AppLastChangedBy = _userId,
            UserId = _userId,
            ExternalId = "4321",
            AccountMask = "***4321",
            DisplayName = "Vanguard Trust 401k",
            CurrentBalance = 500,
            OfficialName = "Vanguard Total Trust 401k",
            Subtype = "Retirement",
            IsExternalApiImport = true,
            LastApiSyncTime = DateTimeOffset.UtcNow,
            ConnectorId = _connectorId,
            OriginalInsert = DateTimeOffset.UtcNow,
        };

        Guid newAccountId = await _financialAccountRepository.AddAccountAsync(account);

        var deleteResult = await _financialAccountRepository.DeleteAccountAsync(account);
        deleteResult.Should().BeTrue();

        var result = await _financialAccountRepository.GetAccountByIdAsync(_userId, newAccountId);
        result.Should().BeNull();
    }

    public async Task InitializeAsync()
    {
        var registrationRepository = new RegistrationRepository(_dbContext);
        await TestHelpers.SetUpBaseRecords(_accountId, _userId, registrationRepository);

        AccountConnectorEntity data = new AccountConnectorEntity
        {
            AppLastChangedBy = _userId,
            UserId = _userId,
            InstitutionId = "123",
            InstitutionName = "Morgan Stanley",
            DateConnected = DateTimeOffset.UtcNow,
            EncryptedAccessToken = "token",
            TransactionSyncCursor = "",
            Id = _connectorId,
            OriginalInsert = DateTimeOffset.UtcNow,
        };
          await _accountConnectorRepository.InsertAccountConnectorRecord(data);
    }

    public async Task DisposeAsync()
    {
        await TestHelpers.TearDownBaseRecords(_userId, _accountId, _baseRepository);
        string accountConnectorDelete = "DELETE FROM account_connector WHERE user_id = @user_id";
        await _baseRepository.DeleteAsync(accountConnectorDelete, new { user_id = _userId });

        string financialAccountDelete = "DELETE FROM financial_account WHERE user_id = @user_id";
        await _baseRepository.DeleteAsync(financialAccountDelete, new { user_id = _userId });
    }
}