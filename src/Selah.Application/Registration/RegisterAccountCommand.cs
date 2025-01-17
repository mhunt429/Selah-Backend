using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Selah.Application.ApplicationUser;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.Registration;
using Selah.Infrastructure;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Registration;

public class RegisterAccount
{
    public class Command : IRequest<AccessTokenResponse>
    {
        public AccountRegistrationRequest Request { get; set; }
    }

    public class Handler : IRequestHandler<Command, AccessTokenResponse>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<Handler> _logger;

        public Handler(IRegistrationRepository registrationRepository, ICryptoService cryptoService,
            IPasswordHasherService passwordHasherService, ITokenService tokenService, ILogger<Handler> logger)
        {
            _registrationRepository = registrationRepository;
            _cryptoService = cryptoService;
            _passwordHasherService = passwordHasherService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AccessTokenResponse> Handle(Command command, CancellationToken cancellationToken)
        {
            Guid userId = await _registrationRepository.RegisterAccount(MapRequestToSql(command.Request));

            AccessTokenResponse accessTokenResponse = _tokenService.GenerateAccessToken(userId.ToString());

            _logger.LogInformation("User with id {id} was successfully created", userId);
            return accessTokenResponse;
        }

        private RegistrationSql MapRequestToSql(AccountRegistrationRequest request)
        {
            return new RegistrationSql
            {
                AccountId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Password = _passwordHasherService.HashPassword(request.Password),
                EncryptedEmail = _cryptoService.Encrypt(request.Email),
                EncryptedName = _cryptoService.Encrypt($"{request.FirstName}|{request.LastName}"),
                EncryptedPhone = _cryptoService.Encrypt(request.PhoneNumber),
                PhoneVerified = false,
                EmailVerified = false,
                AccountName = request.AccountName,
                EmailHash = _cryptoService.HashValue(request.Email),
            };
        }
    }
}