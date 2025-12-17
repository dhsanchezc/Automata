using MediatR;

namespace Automata.Application.Assets.Commands;

public record CreateAssetCommand(
    string Name,
    string Description
    ) : IRequest<int>;