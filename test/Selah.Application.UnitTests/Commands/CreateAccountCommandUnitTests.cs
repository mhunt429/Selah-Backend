using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using Selah.Application.Commands;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.Registration;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.UnitTests.Commands;

public class CreateAccountCommandUnitTests
{
    private readonly Mock<IRegistrationRepository> _registrationRepository = new();
    private readonly Mock<ICryptoService> _cryptoService = new();
    private readonly Mock<IPasswordHasherService> _passwordHasherService = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<ILogger<RegisterAccountCommand.Handler>> _logger = new();

    private RegisterAccountCommand.Command _command;
    private RegisterAccountCommand.Handler _handler;

    private Guid _userId = Guid.NewGuid();

    public CreateAccountCommandUnitTests()
    {
        _registrationRepository.Setup(x => x.RegisterAccount(It.IsAny<RegistrationSql>()))
            .ReturnsAsync(_userId);

        _handler = new RegisterAccountCommand.Handler(_registrationRepository.Object, _cryptoService.Object,
            _passwordHasherService.Object, _tokenService.Object, _logger.Object);

        _tokenService.Setup(x => x.GenerateAccessToken(_userId.ToString()))
            .Returns(new AccessTokenResponse
            {
                AccessToken = "token",
                RefreshToken = "refreshToken",
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(5),
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(10),
            });
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("testing123@", "@assword!")]
    public void CreateAccountCommand_ShouldValidateAgainstEmptyValue(string email, string password)
    {
        _command = new RegisterAccountCommand.Command
        {
            FirstName = "",
            LastName = "",
            Email = email,
            Password = password,
        };

        var validator = new RegisterAccountCommand.Validator();
        TestValidationResult<RegisterAccountCommand.Command>? result = validator.TestValidate(_command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.ShouldHaveValidationErrorFor(command => command.FirstName);
        result.ShouldHaveValidationErrorFor(command => command.LastName);
        result.ShouldHaveValidationErrorFor(command => command.Email);
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }

    [Fact]
    public void CreateAccountCommand_ShouldAllowValidInput()
    {
        _command = new RegisterAccountCommand.Command
        {
            FirstName = "Hingle",
            LastName = "McCringleberry",
            Email = "testing123@test.com",
            Password = "AStrongPassword!42",
            PasswordConfirmation = "AStrongPassword!42",
        };

        var validator = new RegisterAccountCommand.Validator();
        TestValidationResult<RegisterAccountCommand.Command>? result = validator.TestValidate(_command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAccountCommand_ShouldCreateAccount()
    {
        _command = new RegisterAccountCommand.Command
        {
            FirstName = "Hingle",
            LastName = "McCringleberry",
            Email = "testing123@test.com",
            Password = "AStrongPassword!42",
            PasswordConfirmation = "AStrongPassword!42",
        };

        var result = await _handler.Handle(_command, CancellationToken.None);
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Errors.Should().BeEmpty();

        result.Data.Should().NotBeNull();
        result?.Data?.AccessToken.Should().Be("token");
        result?.Data?.RefreshToken.Should().Be("refreshToken");
    }
}