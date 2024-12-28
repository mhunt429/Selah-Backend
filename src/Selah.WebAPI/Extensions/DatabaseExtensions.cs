using Selah.Infrastructure.Repository;

namespace Selah.WebAPI.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository, BaseRepository>();

        return services;
    }
}