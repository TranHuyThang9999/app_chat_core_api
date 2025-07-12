using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.IUnitOfWork;
using PaymentCoreServiceApi.Infrastructure.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.Repositories.UnitOfWork;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Register Repositories
builder.Services.AddScoped(typeof(IBaseWriteOnlyRepository<>), typeof(EfBaseWriteOnlyRepository<>));
builder.Services.AddScoped<IUserWriteRepository, UserWriteRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();