using AutoMapper;
using Automata.Application.Assets.Dtos;
using Automata.Application.Assets.Queries;
using Automata.Domain.Ports.Repositories;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class GetAssetByIdHandler : IRequestHandler<GetAssetByIdQuery, AssetDto?>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;

    public GetAssetByIdHandler(IAssetRepository assetRepository, IMapper mapper)
    {
        _assetRepository = assetRepository;
        _mapper = mapper;
    }
    
    public async Task<AssetDto?> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(request.Id, cancellationToken);
        return asset == null ? null : _mapper.Map<AssetDto>(asset);
    }
}