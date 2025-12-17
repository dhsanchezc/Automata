using Automata.Application.Assets.Commands;
using Automata.Domain.Ports.Repositories;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class DeleteAssetHandler : IRequestHandler<DeleteAssetCommand, bool>
{
    private readonly IAssetRepository _assetRepository;

    public DeleteAssetHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<bool> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.FindAsync(request.Id, cancellationToken);
        if (asset == null) return false;

        _assetRepository.Delete(asset);

        return await _assetRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
