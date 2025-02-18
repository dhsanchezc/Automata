using Automata.Application.Assets.Queries;
using Automata.Domain.Assets;
using Automata.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Automata.Application.Assets.Handlers;

public class GetAllAssetsHandler : IRequestHandler<GetAllAssetsQuery, List<Asset>>
{
    private readonly ApplicationDbContext _db;
    public GetAllAssetsHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Asset>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
    {
        return await _db.Assets.ToListAsync(cancellationToken);
    }
}