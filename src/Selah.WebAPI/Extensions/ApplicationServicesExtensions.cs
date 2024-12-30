using HashidsNet;
using Selah.Application.Services;
using Selah.Application.Services.Interfaces;
using Selah.Infrastructure;
using Selah.Infrastructure.Services;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.WebAPI.Extensions;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IPasswordHasherService, PasswordHasherService>()
            .AddScoped<ICryptoService, CryptoService>()
            .AddScoped<IRegistrationHttpService, RegistrationHttpService>();
    }
}