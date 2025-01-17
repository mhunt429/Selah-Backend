using MediatR;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Identity;

public class UserLogin
{
    public class Command : IRequest<Response>
    {
        public LoginRequest LoginRequest { get; set; }
    }

    public class Response
    {
        public AccessTokenResponse Data { get; set; }
    }

    public class Handler : IRequestHandler<Command, Response>
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

        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var loginRequest = command.LoginRequest;

            string hashedEmail = _cryptoService.HashValue(loginRequest.Email);
            ApplicationUserSql? dbUser = await _repository.GetUserByEmail(hashedEmail);

            if (dbUser == null) return null;

            if (_passwordHasherService.VerifyPassword(loginRequest.Password, dbUser.Password))
            {
                var response = new Response
                {
                    Data = _tokenService.GenerateAccessToken(dbUser.Id.ToString())
                };
                return response;
            }

            return null;
        }
    }
}