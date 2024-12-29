using Selah.Infrastructure.Repository;

namespace Selah.WebAPI.Extensions;

public static class DatabaseExtensions
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository, BaseRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
    }
}