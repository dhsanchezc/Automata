using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Domain.Ports.Repositories;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, bool>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;
    
    public UpdateAssetHandler(IAssetRepository assetRepository, IMapper mapper)
    {
        _assetRepository = assetRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) return false;

        _mapper.Map(request, asset);

        await _assetRepository.UpdateAsync(asset);
        return true;
    }
}