using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Automata.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Asset> Assets { get; set; } = null!;

    // IUnitOfWork Implementation
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        return true;
    }
}