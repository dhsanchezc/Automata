using AutoMapper;
using Automata.Application.Assets.Dtos;
using Automata.Application.Assets.Queries;
using Automata.Application.Interfaces;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class GetAssetByIdHandler : IRequestHandler<GetAssetByIdQuery, AssetDto?>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetAssetByIdHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<AssetDto?> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var asset = await _db.Assets.FindAsync(request.Id);
        return asset == null ? null : _mapper.Map<AssetDto>(asset);
    }
}