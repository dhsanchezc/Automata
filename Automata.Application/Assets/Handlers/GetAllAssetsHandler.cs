using AutoMapper;
using Automata.Application.Assets.Dtos;
using Automata.Application.Assets.Queries;
using Automata.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Automata.Application.Assets.Handlers;

public class GetAllAssetsHandler : IRequestHandler<GetAllAssetsQuery, List<AssetDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetAllAssetsHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<AssetDto>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
    {
        var assets = await _db.Assets.ToListAsync(cancellationToken);
        return _mapper.Map<List<AssetDto>>(assets);
    }
}