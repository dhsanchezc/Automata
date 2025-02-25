using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Application.Interfaces;
using Automata.Domain.Assets;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class CreateAssetHandler : IRequestHandler<CreateAssetCommand, int>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateAssetHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = _mapper.Map<Asset>(request);

        _db.Assets.Add(asset);
        await _db.SaveChangesAsync(cancellationToken);

        return asset.Id;
    }
}