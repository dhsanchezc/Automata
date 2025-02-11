using Automata.Domain.Assets;
using Automata.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with In-Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AutomataDb"));

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

var assets = app.MapGroup("/assets");

// Get all assets
assets.MapGet("/", async (ApplicationDbContext db) => await db.Assets.ToListAsync());

// Get a single asset by ID
assets.MapGet("/{id:int}", async (int id, ApplicationDbContext db) =>
    await db.Assets.FindAsync(id) is Asset asset ? Results.Ok(asset) : Results.NotFound());

// Create a new asset
assets.MapPost("/", async (Asset asset, ApplicationDbContext db) =>
{
    db.Assets.Add(asset);
    await db.SaveChangesAsync();
    return Results.Created($"/assets/{asset.Id}", asset);
});

// Update an asset
assets.MapPut("/{id:int}", async (int id, Asset updatedAsset, ApplicationDbContext db) =>
{
    var asset = await db.Assets.FindAsync(id);
    if (asset is null) return Results.NotFound();

    asset.Name = updatedAsset.Name;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete an asset
assets.MapDelete("/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var asset = await db.Assets.FindAsync(id);
    if (asset is null) return Results.NotFound();

    db.Assets.Remove(asset);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();