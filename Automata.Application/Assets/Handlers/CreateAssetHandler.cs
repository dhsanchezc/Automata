using Automata.Application.Assets.Commands;
using Automata.Domain.Assets;
using Automata.Infrastructure;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class CreateAssetHandler : IRequestHandler<CreateAssetCommand, int>
{
    private readonly ApplicationDbContext _db;
    public CreateAssetHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = new Asset { Name = request.Name };
        _db.Assets.Add(asset);
        await _db.SaveChangesAsync(cancellationToken);
        return asset.Id;
    }
}