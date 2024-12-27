using Selah.Core.Configuration;

namespace Selah.WebAPI.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        AwsConfig awsConfig = configuration.GetSection("AwsConfig").Get<AwsConfig>();

        if (awsConfig == null)
        {
            throw new ArgumentNullException(nameof(awsConfig));
        }

        services.AddSingleton(awsConfig);

        PlaidConfig plaidConfig = configuration.GetSection("PlaidConfig").Get<PlaidConfig>();
        if (plaidConfig == null)
        {
            throw new ArgumentNullException(nameof(plaidConfig));
        }

        services.AddSingleton(plaidConfig);

        SecurityConfig securityConfig = configuration.GetSection("SecurityConfig").Get<SecurityConfig>();
        if (securityConfig == null)
        {
            throw new ArgumentNullException(nameof(securityConfig));
        }

        services.AddSingleton(securityConfig);
        TwilioConfig twilioConfig = configuration.GetSection("TwilioConfig").Get<TwilioConfig>();
        if (twilioConfig == null)
        {
            throw new ArgumentNullException(nameof(twilioConfig));
        }

        return services;
    }
}