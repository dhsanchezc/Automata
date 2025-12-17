using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Ports.Repositories;
using MediatR;

namespace Automata.Application.Assets.Handlers;

public class AddMaintenanceRecordHandler : IRequestHandler<AddMaintenanceRecordCommand, bool>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;

    public AddMaintenanceRecordHandler(IAssetRepository assetRepository, IMapper mapper)
    {
        _assetRepository = assetRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddMaintenanceRecordCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.FindAsync(request.AssetId, cancellationToken);
        if (asset == null) return false;

        var record = _mapper.Map<MaintenanceRecord>(request.RecordDto);

        try
        {
            asset.ScheduleMaintenance(record);
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        await _assetRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return true;
    }
}
