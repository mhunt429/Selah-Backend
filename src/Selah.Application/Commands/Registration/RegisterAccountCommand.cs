using Microsoft.Extensions.Logging;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.Registration;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Commands;

public interface IRegisterAccountCommand
{
    Task<AccessTokenResponse> Register(AccountRegistrationRequest request);
}

public class RegisterAccountCommand : IRegisterAccountCommand
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly ICryptoService _cryptoService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<RegisterAccountCommand> _logger;


    public RegisterAccountCommand(IRegistrationRepository registrationRepository, ICryptoService cryptoService,
        IPasswordHasherService passwordHasherService, ITokenService tokenService,
        ILogger<RegisterAccountCommand> logger)
    {
        _registrationRepository = registrationRepository;
        _cryptoService = cryptoService;
        _passwordHasherService = passwordHasherService;
        _tokenService = tokenService;
        _logger = logger;
    }


    public async Task<AccessTokenResponse> Register(AccountRegistrationRequest request)
    {
        Guid userId = await _registrationRepository.RegisterAccount(MapRequestToSql(request));

        AccessTokenResponse accessTokenResponse = _tokenService.GenerateAccessToken(userId.ToString());

        _logger.LogInformation("User with id {id} was successfully created", userId);
        return accessTokenResponse;
    }

    private RegistrationSql MapRequestToSql(AccountRegistrationRequest request)
    {
        return new RegistrationSql
        {
            Username = request.Username,
            Password = _passwordHasherService.HashPassword(request.Password),
            EncryptedEmail = _cryptoService.Encrypt(request.Email),
            EncryptedName = _cryptoService.Encrypt($"{request.FirstName}|{request.LastName}"),
            EncryptedPhone = _cryptoService.Encrypt(request.PhoneNumber),
            PhoneVerified = false,
            EmailVerified = false,
            AccountName = request.AccountName
        };
    }
}