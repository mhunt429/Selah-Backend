using Selah.Application.Commands;

namespace Selah.WebAPI.Extensions;

public static class CommandUseCaseExtensions
{
    public static void RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<IRegisterAccountCommand, RegisterAccountCommand>();
    }
}