using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Infrastructure.Extensions;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using PaymentCoreServiceApi.Features.Auth;
using PaymentCoreServiceApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Register Services
builder.Services
    .AddRepositories()
    .AddJwtAuthentication(builder.Configuration)
    .AddHttpContextServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Use JWT Middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();