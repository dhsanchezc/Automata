using Automata.Application.Assets.Commands;
using Automata.Infrastructure;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, bool>
{
    private readonly ApplicationDbContext _db;
    public UpdateAssetHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _db.Assets.FindAsync(request.Id);
        if (asset == null) return false;

        asset.Name = request.Name;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
