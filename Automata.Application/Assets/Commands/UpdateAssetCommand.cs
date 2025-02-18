using MediatR;

namespace Automata.Application.Assets.Commands;

public record UpdateAssetCommand(int Id, string Name) : IRequest<bool>;