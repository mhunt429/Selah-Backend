using Selah.Application.Mappings;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Queries.ApplicationUser;

public interface IUserLoginCommand
{
    Task<AccessTokenResponse?> Login(LoginRequest loginRequest);
}

public class UserLoginCommand : IUserLoginCommand
{
    private readonly IApplicationUserRepository _repository;
    private readonly ICryptoService _cryptoService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly ITokenService _tokenService;

    public UserLoginCommand(IApplicationUserRepository repository, ICryptoService cryptoService,
        IPasswordHasherService passwordHasherService, ITokenService tokenService)
    {
        _repository = repository;
        _cryptoService = cryptoService;
        _passwordHasherService = passwordHasherService;
        _tokenService = tokenService;
    }

    public async Task<AccessTokenResponse?> Login(LoginRequest loginRequest)
    {
        string hashedEmail = _cryptoService.HashValue(loginRequest.Email);
        ApplicationUserSql? dbUser = await _repository.GetUserByEmail(hashedEmail);

        if (dbUser == null) return null;

        if (_passwordHasherService.VerifyPassword(loginRequest.Password, dbUser.Password))
        {
            return _tokenService.GenerateAccessToken(dbUser.Id.ToString());
        }

        return null;
    }
}