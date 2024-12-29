using FluentAssertions;
using FluentValidation.TestHelper;
using Selah.Application.Commands;

namespace Selah.Application.UnitTests.Commands;

public class CreateAccountCommandUnitTests
{
    public RegisterAccountCommand.Command _command;


    public CreateAccountCommandUnitTests()
    {
       
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
            Password =password,
        };
        
        var validator = new RegisterAccountCommand.Validator();
        TestValidationResult<RegisterAccountCommand.Command>? result = validator.TestValidate(_command);
        result.IsValid.Should().BeFalse();
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
            Password ="AStrongPassword!42",
            PasswordConfirmation = "AStrongPassword!42",
        };
        
        var validator = new RegisterAccountCommand.Validator();
        TestValidationResult<RegisterAccountCommand.Command>? result = validator.TestValidate(_command);
        result.IsValid.Should().BeTrue();
    }
}