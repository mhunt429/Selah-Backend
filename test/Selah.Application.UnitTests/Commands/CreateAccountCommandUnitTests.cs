using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Selah.Application.Registration;
using Selah.Application.Validators;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models;
using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Core.Models.Entities.UserAccount;
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
    private readonly Mock<IValidator<AccountRegistrationRequest>> _validatorMock = new();


    private RegisterAccount.Handler _handler;


    private Guid _userId = Guid.NewGuid();

    public CreateAccountCommandUnitTests()
    {
        _registrationRepository
            .Setup(x => x.RegisterAccount(It.IsAny<UserAccountEntity>(), It.IsAny<ApplicationUserEntity>()))
            .ReturnsAsync(_userId);

        _validatorMock = new Mock<IValidator<AccountRegistrationRequest>>();
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AccountRegistrationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _handler = new RegisterAccount.Handler(_registrationRepository.Object, _cryptoService.Object,
            _passwordHasherService.Object, _tokenService.Object, _logger.Object, _validatorMock.Object);

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
        result.data.Should().NotBeNull();
        result.status.Should().Be(ResultStatus.Success);

        result.data?.AccessToken.Should().Be("token");
        result.data?.RefreshToken.Should().Be("refreshToken");
        result.data?.AccessTokenExpiration.Should().BeAfter(DateTime.MinValue);
        result.data?.RefreshTokenExpiration.Should().BeAfter(DateTime.MinValue);
    }
}