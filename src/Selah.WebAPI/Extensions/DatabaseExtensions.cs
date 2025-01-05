using System.Diagnostics.CodeAnalysis;
using Selah.Infrastructure.Repository;

namespace Selah.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class DatabaseExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository, BaseRepository>()
            .AddScoped<IRegistrationRepository, RegistrationRepository>()
            .AddScoped<IApplicationUserRepository, AppUserRepository>();
        
        return services;
    }
}