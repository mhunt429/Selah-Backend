using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Hangfire;
using Hangfire.PostgreSql;
using Selah.Core.Configuration;
using Selah.Infrastructure.Services;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterRepositories()
            .RegisterCommands()
            .RegisterQueries()
            .AddApplicationServices()
            .AddHttpClients(configuration)
            .RegisterHangfire(configuration)
            ;
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

    public static IServiceCollection RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetValue<string>("SelahDbConnectionString");
        services.AddHangfire(x => 
           x.UseRecommendedSerializerSettings()
               .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString))
          );
        services.AddHangfireServer();
        return services;
    }
}