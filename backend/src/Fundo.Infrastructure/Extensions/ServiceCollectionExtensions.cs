using Fundo.Domain.Entities;
using Fundo.Infrastructure.Context;
using Fundo.Infrastructure.Repositories;
using Fundo.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fundo.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment env)
    {
        services.AddDbContext<LoanDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IRepository<Loan>, LoanRepository>();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LoanDbContext>();

            if (!env.IsEnvironment("Test"))
            {
                context.Database.Migrate();
            }

            LoanDbContextSeed.SeedAsync(context).Wait();
        }

        return services;
    }
}