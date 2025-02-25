using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Application.Interfaces;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateAssetHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _db.Assets.FindAsync(request.Id);
        if (asset == null) return false;

        _mapper.Map(request, asset);

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}