using System.Data;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.Registration;
using Selah.Infrastructure;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Commands;

public class RegisterAccountCommand
{
    public class Command : AccountRegistrationRequest, IRequest<BaseHttpResponse<AccessTokenResponse>>
    {
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Invalid email format.");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Password).Equal(x => x.PasswordConfirmation).WithMessage("Passwords don't match");
        }
    }

    public class Handler : IRequestHandler<Command, BaseHttpResponse<AccessTokenResponse>>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private ICryptoService _cryptoService;
        private IPasswordHasherService _passwordHasherService;
        private ITokenService _tokenService;
        private ILogger<Handler> _logger;

        public Handler(IRegistrationRepository registrationRepository, ICryptoService cryptoService,
            IPasswordHasherService passwordHasherService, ITokenService tokenService,
            ILogger<Handler> logger)
        {
            _registrationRepository = registrationRepository;
            _cryptoService = cryptoService;
            _passwordHasherService = passwordHasherService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<BaseHttpResponse<AccessTokenResponse>> Handle(Command request,
            CancellationToken cancellationToken)
        {
            var validator = new Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new BaseHttpResponse<AccessTokenResponse>
                {
                    StatusCode = 400,
                    Data = null,
                    Errors = validationResult.Errors
                };
            }

            Guid userId = await _registrationRepository.RegisterAccount(MapRequestToSql(request));

            AccessTokenResponse accessTokenResponse = _tokenService.GenerateAccessToken(userId.ToString());

            _logger.LogInformation("User with id {id} was successfully created", userId);
            return new BaseHttpResponse<AccessTokenResponse>
            {
                StatusCode = 200,
                Data = accessTokenResponse
            };
        }

        private RegistrationSql MapRequestToSql(AccountRegistrationRequest request)
        {
            return new RegistrationSql
            {
                Username = request.Username,
                Password = _passwordHasherService.HashPassword(request.Password),
                EncryptedName = _cryptoService.Encrypt($"{request.FirstName}|{request.LastName}"),
                EncryptedPhone = _cryptoService.Encrypt(request.PhoneNumber),
                PhoneVerified = false,
                EmailVerified = false,
                AccountName = request.AccountName
            };
        }
    }
}