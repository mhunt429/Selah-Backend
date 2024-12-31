namespace Selah.WebAPI.Extensions;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterRepositories()
            .RegisterCommands()
            .RegisterQueries()
            .AddApplicationServices();
    }
}