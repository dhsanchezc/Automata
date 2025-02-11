using Automata.Domain.Assets;
using Microsoft.EntityFrameworkCore;

namespace Automata.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Asset> Assets { get; set; } = null!;
}