using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Common;
using Automata.Domain.Ports.Repositories;
using Automata.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Automata.Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly ApplicationDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public AssetRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Asset Add(Asset asset)
    {
        return _context.Assets.Add(asset).Entity;
    }

    public Asset Update(Asset asset)
    {
        return _context.Assets.Update(asset).Entity;
    }

    public async Task<Asset?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .Include(a => a.MaintenanceRecords)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Asset?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
    }

    public async Task<List<Asset>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .Include(a => a.MaintenanceRecords)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Delete(Asset asset)
    {
        _context.Assets.Remove(asset);
    }
}
