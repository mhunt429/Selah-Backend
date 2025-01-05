using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Selah.Infrastructure;
using Selah.WebAPI.Extensions;
using Selah.WebAPI.Middleware;


namespace Selah.WebAPI;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure middleware and application behavior
        ConfigureApp(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddSingleton<IDbConnectionFactory>(provider =>
        {
            var connectionString = configuration.GetValue<string>("SelahDbConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException("No connection string configured.");
            }

            return new SelahDbConnectionFactory(connectionString);
        });

        builder.Services.AddConfiguration(configuration);
        builder.Services.AddDependencies(configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        ConfigureAuthentication(builder.Services, configuration);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        string jwtSecret = configuration["SecurityConfig:JwtSecret"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "selah-api",
                ValidAudience = "selah-api",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
    }

    private static void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandler>();
        app.MapControllers();
    }
}