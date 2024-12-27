using HashidsNet;

namespace Selah.WebAPI.Extensions;

public static class HashIdsExtensions
{
    public static IServiceCollection RegisterHashIds(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHashids>(_ => new Hashids(configuration["HASH_ID_SALT"], minHashLength: 24));
        return services;
    }
}