using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Selah.Application.Registration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Core.Models.Sql.UserAccount;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.UnitTests.Commands;

public class CreateAccountCommandUnitTests
{
    private readonly Mock<IRegistrationRepository> _registrationRepository = new();
    private readonly Mock<ICryptoService> _cryptoService = new();
    private readonly Mock<IPasswordHasherService> _passwordHasherService = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<ILogger<RegisterAccount.Handler>> _logger = new();

    private RegisterAccount.Handler _handler;


    private Guid _userId = Guid.NewGuid();

    public CreateAccountCommandUnitTests()
    {
        _registrationRepository
            .Setup(x => x.RegisterAccount(It.IsAny<UserAccountSql>(), It.IsAny<ApplicationUserSql>()))
            .ReturnsAsync(_userId);

        _handler = new RegisterAccount.Handler(_registrationRepository.Object, _cryptoService.Object,
            _passwordHasherService.Object, _tokenService.Object, _logger.Object);

        _tokenService.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>()))
            .Returns(new AccessTokenResponse
            {
                AccessToken = "token",
                RefreshToken = "refreshToken",
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(5),
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(10),
            });
    }


    [Fact]
    public async Task Register_ShouldReturnAccessToken()
    {
        var command = new RegisterAccount.Command
        {
            FirstName = "Hingle",
            LastName = "McCringleberry",
            Email = "testing123@test.com",
            Password = "AStrongPassword!42",
            PasswordConfirmation = "AStrongPassword!42",
        };

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("token");
        result.RefreshToken.Should().Be("refreshToken");
        result.AccessTokenExpiration.Should().BeAfter(DateTime.MinValue);
        result.RefreshTokenExpiration.Should().BeAfter(DateTime.MinValue);
    }
}