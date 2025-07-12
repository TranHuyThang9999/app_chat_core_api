using Microsoft.Extensions.DependencyInjection;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.Repositories.Write;

namespace PaymentCoreServiceApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Register Base Repositories
        services.AddScoped(typeof(IBaseWriteOnlyRepository<>), typeof(EfBaseWriteOnlyRepository<>));
        
        // Register Domain Specific Repositories
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        
        return services;
    }
}
