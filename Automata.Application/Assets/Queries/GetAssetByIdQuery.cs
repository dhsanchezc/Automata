using Automata.Domain.Assets;
using MediatR;

namespace Automata.Application.Assets.Queries;

public record GetAssetByIdQuery(int Id) : IRequest<Asset?>;