using Selah.Application.Commands;
using Selah.Application.Queries.ApplicationUser;

namespace Selah.WebAPI.Extensions;

public static class CommandUseCaseExtensions
{
    public static IServiceCollection RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<IRegisterAccountCommand, RegisterAccountCommand>();
        return services;
    }

    public static IServiceCollection RegisterQueries(this IServiceCollection services)
    {
        services.AddScoped<IGetUserByIdQuery, GetUserByIdQuery>();
        return services;
        
    }
}