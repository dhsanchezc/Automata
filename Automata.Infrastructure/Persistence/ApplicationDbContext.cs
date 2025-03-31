using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Automata.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;

    // IUnitOfWork Implementation
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        return true;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>()
            .HasMany(e => e.MaintenanceRecords)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Asset>()
            .Navigation(nameof(Asset.MaintenanceRecords))
            .HasField("_maintenanceRecords");

        modelBuilder.Entity<MaintenanceRecord>()
            .Property(m => m.Type)
            .HasConversion<string>();

        modelBuilder.Entity<MaintenanceRecord>()
            .Property(m => m.Status)
            .HasConversion<string>();
    }
}