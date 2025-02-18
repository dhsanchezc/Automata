using System;
using Automata.Application.Assets.Commands;
using Automata.Infrastructure;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class DeleteAssetHandler : IRequestHandler<DeleteAssetCommand, bool>
{
    private readonly ApplicationDbContext _db;
    public DeleteAssetHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _db.Assets.FindAsync(request.Id);
        if (asset == null) return false;

        _db.Assets.Remove(asset);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
