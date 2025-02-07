using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Selah.Infrastructure;
using Selah.Infrastructure.Repository;

namespace Selah.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class DatabaseExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        
        string? connectionString = configuration.GetValue<string>("SelahDbConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string is missing. Ensure 'ConnectionStrings__DefaultConnection' is set as an environment variable.");
        }
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IBaseRepository, BaseRepository>()
            .AddScoped<IRegistrationRepository, RegistrationRepository>()
            .AddScoped<IApplicationUserRepository, AppUserRepository>()
            .AddScoped<IAccountConnectorRepository, AccountConnectorRepository>()
            .AddScoped<IFinancialAccountRepository, FinancialAccountRepository>();

        return services;
    }
}