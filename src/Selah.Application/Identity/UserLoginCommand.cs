using MediatR;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Identity;

public class UserLogin
{
    public class Command : IRequest<AccessTokenResponse>
    {
        public LoginRequest LoginRequest { get; set; }
    }
    

    public class Handler : IRequestHandler<Command, AccessTokenResponse>
    {
        private readonly IApplicationUserRepository _repository;
        private readonly ICryptoService _cryptoService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ITokenService _tokenService;

        public Handler(
            IApplicationUserRepository repository,
            ICryptoService cryptoService,
            IPasswordHasherService passwordHasherService,
            ITokenService tokenService)
        {
            _repository = repository;
            _cryptoService = cryptoService;
            _passwordHasherService = passwordHasherService;
            _tokenService = tokenService;
        }

        public async Task<AccessTokenResponse> Handle(Command command, CancellationToken cancellationToken)
        {
            var loginRequest = command.LoginRequest;

            string hashedEmail = _cryptoService.HashValue(loginRequest.Email);
            ApplicationUserEntity? dbUser = await _repository.GetUserByEmail(hashedEmail);

            if (dbUser == null) return null;

            if (_passwordHasherService.VerifyPassword(loginRequest.Password, dbUser.Password))
            {
                return _tokenService.GenerateAccessToken(dbUser.Id);
            }

            return null;
        }
    }
}