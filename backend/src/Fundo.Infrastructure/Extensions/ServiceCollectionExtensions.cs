using Fundo.Domain.Entities;
using Fundo.Infrastructure.Context;
using Fundo.Infrastructure.Repositories;
using Fundo.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LoanDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IRepository<Loan>, LoanRepository>();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LoanDbContext>();

            context.Database.Migrate();


            LoanDbContextSeed.SeedAsync(context).Wait();
        }

        return services;
    }
}