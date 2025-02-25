using Automata.Application.Interfaces;
using Automata.Domain.Assets;

using Microsoft.EntityFrameworkCore;

namespace Automata.Infrastructure;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // EF Core DbSet
    public DbSet<Asset> Assets { get; set; } = null!;

    // Implement SaveChangesAsync
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}