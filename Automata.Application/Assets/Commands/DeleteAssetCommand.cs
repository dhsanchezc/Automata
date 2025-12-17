using MediatR;

namespace Automata.Application.Assets.Commands;

public record DeleteAssetCommand(int Id) : IRequest<bool>;