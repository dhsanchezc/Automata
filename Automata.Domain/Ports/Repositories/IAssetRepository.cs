using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Common;

namespace Automata.Domain.Ports.Repositories;

public interface IAssetRepository : IRepository<Asset>
{
    Asset Add(Asset asset);
    Asset Update(Asset asset);
    Task<Asset?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Asset?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Asset>> GetAllAsync(CancellationToken cancellationToken = default);
    void Delete(Asset asset);
}
