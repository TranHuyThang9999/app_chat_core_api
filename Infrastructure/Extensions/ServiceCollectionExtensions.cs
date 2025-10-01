using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Features.Auth;
using PaymentCoreServiceApi.Features.BankAccounts.Commands;
using PaymentCoreServiceApi.Infrastructure.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.Repositories.Write;
using PaymentCoreServiceApi.Middlewares;
using PaymentCoreServiceApi.Services;
using PaymentCoreServiceApi.Services.Security;

namespace PaymentCoreServiceApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Register Base Repositories
        services.AddScoped(typeof(IBaseWriteOnlyRepository<>), typeof(EfBaseWriteOnlyRepository<>));
        
        // Register Domain Specific Repositories
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IBankAccountWriteRepository, BankAccountWriteRepository>();
        services.AddScoped<IBankAccountReadRepository, BankAccountReadRepository>();
        services.AddScoped<IMessageWriteRepository, MessageWriteRepository>();
        services.AddScoped<IMessageReadRepository, MessageReadRepository>();
        services.AddScoped<IPinHasher, PinHasher>();
        services.AddScoped<IConversationWriteRepository, ConversationWriteRepository>();
        services.AddScoped<IConversationReadRepository, ConversationReadRepository>();
        services.AddScoped<IChannelMemberWriteRepository, ChannelMemberWriteRepository>();
        services.AddScoped<IChannelMemberReadRepository, ChannelMemberReadRepository>();
        
        return services;
    }
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    public static IServiceCollection AddHttpContextServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IExecutionContext, CurrentUser>();
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Register JWT Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<JwtMiddleware>();

        // Configure JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        return services;
    }

    public static IServiceCollection AddAppMetrics(this IServiceCollection services)
    {
        services.AddSingleton<IAppMetricsService, AppMetricsService>();
        return services;
    }
}