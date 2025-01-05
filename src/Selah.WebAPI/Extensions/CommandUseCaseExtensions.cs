using System.Diagnostics.CodeAnalysis;
using Selah.Application.Commands;
using Selah.Application.Commands.AccountConnector;
using Selah.Application.Queries.ApplicationUser;

namespace Selah.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class CommandUseCaseExtensions
{
    public static IServiceCollection RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<IRegisterAccountCommand, RegisterAccountCommand>()
            .AddScoped<ICreateLinkTokenCommand, CreateLinkTokenCommand>()
            .AddScoped<IUserLoginCommand, UserLoginCommand>()
            ;
        return services;
    }

    public static IServiceCollection RegisterQueries(this IServiceCollection services)
    {
        services.AddScoped<IGetUserByIdQuery, GetUserByIdQuery>();
        return services;
    }
}