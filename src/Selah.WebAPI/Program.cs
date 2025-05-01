using System.Diagnostics.CodeAnalysis;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Selah.Infrastructure;
using Selah.WebAPI.Extensions;
using Selah.WebAPI.Middleware;
using Hangfire.Dashboard.BasicAuthorization;
using Scalar.AspNetCore;
using Selah.Application.ApplicationUser;
using Selah.Infrastructure.RecurringJobs;

namespace Selah.WebAPI;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        DotNetEnv.Env.Load("../.env");
        // Configure services
        ConfigureServices(builder);

        var app = builder.Build();
        // Configure middleware and application behavior
        ConfigureApp(app, builder.Configuration);

        app.Run();
    }

    private static IServiceCollection ConfigureServices(WebApplicationBuilder builder)
    {
        
        
        var configuration = builder.Configuration;
        
        //Because of the automatic dependency injection with mediatr and we have all of that in the Application Project, we just need to pass in a single IRequest instance
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining(typeof(GetUserById.Query)));

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

        builder.Services.AddOpenApi();

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

        return builder.Services;
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

    private static void ConfigureApp(WebApplication app, IConfiguration configuration)
    {
        if (app.Environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.MapOpenApi();
            
            app.UseSwaggerUI(options => 
                options.SwaggerEndpoint("/openapi/v1.json", "Selah.AppHost.WebAPI")
                );
            app.UseReDoc(options =>
            {
                options.SpecUrl = "/openapi/v1.json";
            });

            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandler>();
        app.MapControllers();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[]
            {
                new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions()
                {
                    SslRedirect = false,
                    RequireSsl = false,
                    LoginCaseSensitive = true,
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = configuration.GetValue<string>("HangfireUsername"),
                            PasswordClear = configuration.GetValue<string>("HangfirePassword")
                        }
                    }
                })
            }
        });

        RegisterHangfireJobs();
    }

    private static void RegisterHangfireJobs()
    {
        RecurringJob.AddOrUpdate<RecurringAccountBalanceUpdateJob>(
            methodCall: job => job.DoWork(),
            cronExpression: Cron.Daily(3) //Run this job daily at 3 AM 
        );
    }
}