using MediatR;
using Microsoft.Extensions.Logging;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Core.Models.Entities.UserAccount;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Registration;

public class RegisterAccount
{
    public class Command : AccountRegistrationRequest, IRequest<AccessTokenResponse>
    {
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
            //Since this is the entry point of the account creation, initialize 2 unique ids for the account and user
            Guid accountId = Guid.CreateVersion7();
            Guid userId = Guid.CreateVersion7();

            UserAccountEntity userAccountEntity = MapRequestToUserAccount(command, accountId, userId);
            ApplicationUserEntity applicationUserEntity = MapRequestToUser(command, accountId, userId);

            await _registrationRepository.RegisterAccount(userAccountEntity, applicationUserEntity);

            AccessTokenResponse accessTokenResponse = _tokenService.GenerateAccessToken(userId);

            _logger.LogInformation("User with id {id} was successfully created", userId);
            return accessTokenResponse;
        }

        private UserAccountEntity MapRequestToUserAccount(AccountRegistrationRequest request, Guid accountId, Guid userId)
        {
            return new UserAccountEntity
            {
                AppLastChangedBy = userId,
                Id = accountId,
                CreatedOn = DateTimeOffset.UtcNow,
                AccountName = request.AccountName,
            };
        }

        private ApplicationUserEntity MapRequestToUser(AccountRegistrationRequest request, Guid accountId, Guid userId)
        {
            return new ApplicationUserEntity
            {
                AppLastChangedBy = userId,
                AccountId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = _passwordHasherService.HashPassword(request.Password),
                EncryptedEmail = _cryptoService.Encrypt(request.Email),
                EncryptedName = _cryptoService.Encrypt($"{request.FirstName}|{request.LastName}"),
                EncryptedPhone = _cryptoService.Encrypt(request.PhoneNumber),
                PhoneVerified = false,
                EmailVerified = false,
                EmailHash = _cryptoService.HashValue(request.Email),
            };
        }
    }
}