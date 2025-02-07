using FluentAssertions;
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
        _repository = new RegistrationRepository(TestHelpers.BuildTestDbContext());
    }

    [Fact]
    public async Task Register_ShouldSaveAccountAndUserRecord()
    {
       

       var result = await TestHelpers.SetUpBaseRecords(accountId, userId, _repository);
       result.Should().NotBeNull();
       result.Item2.Id.Should().Be(userId);
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