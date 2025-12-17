using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Ports.Repositories;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class CreateAssetHandler : IRequestHandler<CreateAssetCommand, int>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;

    public CreateAssetHandler(IAssetRepository assetRepository, IMapper mapper)
    {
        _assetRepository = assetRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = _mapper.Map<Asset>(request);

        _assetRepository.Add(asset);

        await _assetRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return asset.Id;
    }
}