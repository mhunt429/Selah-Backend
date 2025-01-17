using System.Diagnostics.CodeAnalysis;
using HashidsNet;
using Selah.Application.Services;
using Selah.Application.Services.Interfaces;
using Selah.Infrastructure;
using Selah.Infrastructure.Services;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IPasswordHasherService, PasswordHasherService>()
            .AddScoped<ICryptoService, CryptoService>()
            .AddScoped<IAccountConnectorHttpService, AccountConnectorHttpService>()
            ;

        return services;
    }
}