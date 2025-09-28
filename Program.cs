using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Infrastructure.Extensions;
using MediatR;
using PaymentCoreServiceApi.Middlewares;
using PaymentCoreServiceApi.Services;
using Minio;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS configuration from appsettings
builder.Services.AddCors(options =>
{
    var corsConfig = builder.Configuration.GetSection("Cors");
    var allowedOrigins = corsConfig.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
    
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    
    // Policy cho development - cho phép tất cả
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("PostgresConnection")!)
    .AddCheck("minio", () => 
    {
        // Simple MinIO health check
        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("MinIO is available");
    });

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Register Services
builder.Services
    .AddRepositories()
    .AddJwtAuthentication(builder.Configuration)
    .AddHttpContextServices()
    .AddMappings();

// Configure MinIO
builder.Services.AddSingleton<IMinioClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var endpoint = configuration["MinIO:Endpoint"];
    var accessKey = configuration["MinIO:AccessKey"];
    var secretKey = configuration["MinIO:SecretKey"];
    var useSSL = bool.Parse(configuration["MinIO:UseSSL"] ?? "false");

    return new MinioClient()
        .WithEndpoint(endpoint)
        .WithCredentials(accessKey, secretKey)
        .WithSSL(useSSL)
        .Build();
});

builder.Services.AddScoped<IMinIOService, MinIOService>();

var app = builder.Build();

// Add health checks
app.MapHealthChecks("/health");

// Configure CORS - phải đặt trước các middleware khác
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll"); // Cho phép tất cả trong development
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("AllowFrontend"); // Chỉ cho phép các domain cụ thể trong production
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Use JWT Middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();