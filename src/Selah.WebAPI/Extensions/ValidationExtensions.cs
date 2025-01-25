using FluentValidation;
using Selah.Application.Validators;
using Selah.Core.ApiContracts.AccountRegistration;

namespace Selah.WebAPI.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AccountRegistrationRequest>, RegisterAccountValidator>();
        
        return services;
    }
}