using Selah.Application.Commands;
using Selah.Application.Services.Interfaces;
using Selah.Application.Validators;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;

namespace Selah.Application.Services;

public class RegistrationHttpService: IRegistrationHttpService
{
    private readonly IRegisterAccountCommand _registerAccountCommand;

    public RegistrationHttpService(IRegisterAccountCommand registerAccountCommand)
    {
        _registerAccountCommand = registerAccountCommand;
    }
    
    public async Task<BaseHttpResponse<AccessTokenResponse>> RegisterAccount(AccountRegistrationRequest request)
    {
        var validator = new RegisterAccountValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new BaseHttpResponse<AccessTokenResponse>
            {
                StatusCode = 400,
                Data = null,
                Errors = validationResult.Errors.Select(item => item.ErrorMessage)
            };
        }
        
        var accessTokenResponse = await _registerAccountCommand.Register(request);
        
        return new BaseHttpResponse<AccessTokenResponse>
        {
            StatusCode = 200,
            Data = accessTokenResponse
        };
    }
}