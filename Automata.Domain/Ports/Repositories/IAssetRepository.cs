using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Common;

namespace Automata.Domain.Ports.Repositories;

public interface IAssetRepository : IRepository<Asset>
{
    // customized ways to interact with assets?
}
