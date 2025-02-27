using Automata.Application.Interfaces;
using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Ports.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Automata.Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly IApplicationDbContext _db;

    public AssetRepository(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Asset entity, CancellationToken cancellationToken = default)
    {
        await _db.Assets.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Asset entity, CancellationToken cancellationToken = default)
    {
        _db.Assets.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Assets.AsNoTracking().ToListAsync();
    }

    public async Task<Asset> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Assets.FindAsync(new object[] { id }, cancellationToken)
            ?? throw new KeyNotFoundException($"Asset with ID {id} not found.");
    }


    public async Task UpdateAsync(Asset entity, CancellationToken cancellationToken = default)
    {
        _db.Assets.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
