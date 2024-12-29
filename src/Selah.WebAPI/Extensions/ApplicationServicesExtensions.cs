using Selah.Infrastructure;
using Selah.Infrastructure.Services;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.WebAPI.Extensions;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<ICryptoService, CryptoService>();
    }
}