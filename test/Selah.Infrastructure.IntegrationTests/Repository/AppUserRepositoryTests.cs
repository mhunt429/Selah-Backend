using FluentAssertions;
using Selah.Infrastructure.Repository;

namespace Selah.Infrastructure.IntegrationTests.Repository;

public class AppUserRepositoryTests : IAsyncLifetime
{
    private readonly IBaseRepository _baseRepository = new BaseRepository(TestHelpers.TestDbFactory);
    private readonly IApplicationUserRepository _repository;

    private Guid _accountId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();


    public AppUserRepositoryTests()
    {
        _repository = new AppUserRepository(_baseRepository);
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
        await TestHelpers.SetUpBaseRecords(_userId, _accountId, registrationRepository);
    }

    public async Task DisposeAsync()
    {
        await TestHelpers.TearDownBaseRecords(_userId, _accountId, _baseRepository);
    }
}