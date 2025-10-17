using FluentValidation;
using Fundo.Services.Abstractions;
using Fundo.Services.Services;
using Fundo.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection
        AddServices(this IServiceCollection services)
    {
        services.AddScoped<ILoanService, LoanService>();


        services.AddValidatorsFromAssemblyContaining<CreateLoanDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<PaymentDtoValidator>();

        return services;
    }
}