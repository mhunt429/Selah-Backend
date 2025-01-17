using System.Diagnostics.CodeAnalysis;
using Selah.Application.Commands;
using Selah.Application.Commands.AccountConnector;
using Selah.Application.ApplicationUser;

namespace Selah.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class CommandUseCaseExtensions
{
    public static IServiceCollection RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<IRegisterAccountCommand, RegisterAccountCommand>()
            .AddScoped<ICreateLinkTokenCommand, CreateLinkTokenCommand>()
            .AddScoped<IExchangeLinkTokenCommand, ExchangeLinkTokenCommand>()
            ;
        return services;
    }

    public static IServiceCollection RegisterQueries(this IServiceCollection services)
    {
        return services;
    }
}