using AutoMapper;
using Automata.Application.Assets.Dtos;
using Automata.Application.Assets.Queries;
using Automata.Domain.Ports.Repositories;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class GetAllAssetsHandler : IRequestHandler<GetAllAssetsQuery, List<AssetDto>>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;

    public GetAllAssetsHandler(IAssetRepository assetRepository, IMapper mapper)
    {
        _assetRepository = assetRepository;
        _mapper = mapper;
    }

    public async Task<List<AssetDto>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
    {
        var assets = await _assetRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<AssetDto>>(assets);
    }
}