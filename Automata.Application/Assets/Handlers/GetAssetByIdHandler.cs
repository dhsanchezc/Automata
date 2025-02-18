using Automata.Application.Assets.Queries;
using Automata.Domain.Assets;
using Automata.Infrastructure;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class GetAssetByIdHandler : IRequestHandler<GetAssetByIdQuery, Asset?>
{
    private readonly ApplicationDbContext _db;

    public GetAssetByIdHandler(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<Asset?> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _db.Assets.FindAsync(request.Id);
    }
}