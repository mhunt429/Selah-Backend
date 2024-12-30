using FluentAssertions;
using FluentValidation.TestHelper;
using Selah.Application.Validators;
using Selah.Core.ApiContracts.AccountRegistration;

namespace Selah.Application.UnitTests.Validators;

public class RegisterAccountValidatorUnitTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("testing123@", "@assword!")]
    public void Validator_ShouldValidateAgainstInvalidData(string email, string password)
    {
        var validator = new RegisterAccountValidator();
        
        var data =  new AccountRegistrationRequest
        {
            FirstName = "",
            LastName = "",
            Email = email,
            Password = password,
        };
        
      var  result = validator.TestValidate(data);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.ShouldHaveValidationErrorFor(command => command.FirstName);
        result.ShouldHaveValidationErrorFor(command => command.LastName);
        result.ShouldHaveValidationErrorFor(command => command.Email);
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }

    [Fact]
    public void Validator_ShouldAllowValidData()
    {
        var validator = new RegisterAccountValidator();
        var data =  new AccountRegistrationRequest
        {
            FirstName = "Hingle",
            LastName = "McCringleberry",
            Email = "testing123@test.com",
            Password = "AStrongPassword!42",
            PasswordConfirmation = "AStrongPassword!42",
        };
        
        var  result = validator.TestValidate(data);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}