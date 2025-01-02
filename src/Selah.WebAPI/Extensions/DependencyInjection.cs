using System.Net.Http.Headers;
using Selah.Core.Configuration;
using Selah.Infrastructure.Services;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.WebAPI.Extensions;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterRepositories()
            .RegisterCommands()
            .RegisterQueries()
            .AddApplicationServices()
            .AddHttpClients(configuration);
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        PlaidConfig plaidConfig = configuration.GetSection("PlaidConfig").Get<PlaidConfig>();

        services.AddHttpClient<IPlaidHttpService, PlaidHttpService>(config =>
        {
            config.BaseAddress = new Uri(plaidConfig.BaseUrl);
            config.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        });

        return services;
    }
}