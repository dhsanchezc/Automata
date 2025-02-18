using MediatR;

namespace Automata.Application.Assets.Commands;

public record CreateAssetCommand(string Name) : IRequest<int>;