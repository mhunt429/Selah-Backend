using FluentAssertions;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class AppUserRepositoryTests : IAsyncLifetime
{
    private readonly AppDbContext _dbContext =  TestHelpers.BuildTestDbContext();
    
    private readonly IBaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);
    
    private readonly IApplicationUserRepository _repository;

    private Guid _accountId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();


    public AppUserRepositoryTests()
    {
        _repository = new AppUserRepository(_dbContext);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        var result = await _repository.GetUserByIdAsync(_userId);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByEmailHashAsync_ShouldReturnUser()
    {
        var user = await _repository.GetUserByIdAsync(_userId);

        var result = await _repository.GetUserByEmail(user.EmailHash);
        result.Should().NotBeNull();
    }


    public async Task InitializeAsync()
    {
        var registrationRepository = new RegistrationRepository(TestHelpers.BuildTestDbContext());
        await TestHelpers.SetUpBaseRecords( _accountId, _userId, registrationRepository);
    }

    public async Task DisposeAsync()
    {
        await TestHelpers.TearDownBaseRecords(_userId, _accountId, _baseRepository);
    }
}