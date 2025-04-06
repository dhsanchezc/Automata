using System.Reflection;
using Automata.Api.ApiHandlers;
using Automata.Domain.Common;
using Automata.Domain.Ports.Repositories;
using Automata.Infrastructure.Persistence;
using Automata.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext with DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ApplicationDbContext as IUnitOfWork (since it implements IUnitOfWork)
builder.Services.AddScoped<IUnitOfWork>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// Register Repository
builder.Services.AddScoped<IAssetRepository, AssetRepository>();

// Register MediatR for Application Layer
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("Automata.Application"));
});

// Register AutoMapper
builder.Services.AddAutoMapper(Assembly.Load("Automata.Application"));

// Add minimal Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register API Handlers
app.MapGroup("/api/assets")
    .MapAssets();

app.Run();