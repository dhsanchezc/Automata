using Automata.Domain.Assets;
using Microsoft.EntityFrameworkCore; // TODO: Remove from Application layer (UoW)

namespace Automata.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Asset> Assets { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}