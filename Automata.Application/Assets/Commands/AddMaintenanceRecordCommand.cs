using Automata.Application.Assets.Dtos;
using MediatR;

namespace Automata.Application.Assets.Commands;

public record AddMaintenanceRecordCommand(
    int AssetId,
    MaintenanceRecordDto RecordDto
    ) : IRequest<bool>;